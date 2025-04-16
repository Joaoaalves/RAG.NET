using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.AspNetCore.Http;
using RAGNET.Application.DTOs;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.EmbeddingUseCases
{
    public interface IProcessEmbeddingUseCase
    {
        Task<int> Execute(IFormFile file, Workflow workflow);
        IAsyncEnumerable<EmbeddingProgressDTO> ExecuteStreaming(IFormFile file, Workflow workflow, CancellationToken cancellationToken = default);
    }

    public class ProcessEmbeddingUseCase(
        IWorkflowRepository workflowRepository,
        IDocumentProcessingService pdfProcessingService,
        IEmbeddingProcessingService embeddingProcessingService) : IProcessEmbeddingUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IDocumentProcessingService _pdfProcessingService = pdfProcessingService;
        private readonly IEmbeddingProcessingService _embeddingProcessingService = embeddingProcessingService;

        private (string collectionId, Chunker chunkerConfig, EmbeddingProviderConfig embeddingProviderConfig, ConversationProviderConfig conversationProviderConfig) GetConfig(Workflow workflow)
        {
            return (
                workflow.CollectionId.ToString(),
                workflow.Chunker ?? throw new Exception("Chunker not found."),
                workflow.EmbeddingProviderConfig ?? throw new Exception("Embedding Provider not set."),
                workflow.ConversationProviderConfig ?? throw new Exception("Conversation provider config not set.")
            );
        }

        private async Task<Document> ProcessPdfAsync(IFormFile file, Workflow workflow)
        {
            var pdfExtractResult = await _pdfProcessingService.ExtractTextAsync(file);
            return await _pdfProcessingService.CreateDocumentWithPagesAsync(
                file.FileName.Replace(".pdf", ""),
                workflow.Id,
                pdfExtractResult.Pages
            );
        }

        public async Task<int> Execute(IFormFile file, Workflow workflow)
        {
            var (collectionId, chunkerConfig, embeddingProviderConfig, conversationProviderConfig) = GetConfig(workflow);
            var document = await ProcessPdfAsync(file, workflow);

            var chunksToSaveBag = new ConcurrentBag<Chunk>();

            var tasks = document.Pages.Select(async page =>
            {
                var chunks = (await _embeddingProcessingService.ChunkTextAsync(page.Text, chunkerConfig, conversationProviderConfig)).ToList();
                var embeddingResults = await _embeddingProcessingService.GetEmbeddingsAsync(chunks, embeddingProviderConfig);

                var batchToInsert = new List<(string, float[], Dictionary<string, string>)>();

                foreach (var (chunk, vectorId, embedding) in embeddingResults)
                {
                    batchToInsert.Add((vectorId, embedding, []));
                    chunksToSaveBag.Add(new Chunk { Text = chunk, VectorId = vectorId, PageId = page.Id });
                }

                await _embeddingProcessingService.InsertEmbeddingBatchAsync(batchToInsert, collectionId);
                return chunks.Count;
            });

            var results = await Task.WhenAll(tasks);
            var processedChunks = results.Sum();

            await _embeddingProcessingService.AddChunksAsync(chunksToSaveBag.ToList());

            workflow.DocumentsCount++;
            await _workflowRepository.UpdateByApiKey(workflow, workflow.ApiKey);
            return processedChunks;
        }

        public async IAsyncEnumerable<EmbeddingProgressDTO> ExecuteStreaming(IFormFile file, Workflow workflow, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var (collectionId, chunkerConfig, embeddingProviderConfig, conversationProviderConfig) = GetConfig(workflow);
            var document = await ProcessPdfAsync(file, workflow);

            var pageChunksMapping = new ConcurrentBag<(Page page, List<string> chunks)>();
            await Task.WhenAll(document.Pages.Select(async page =>
            {
                var chunks = (await _embeddingProcessingService.ChunkTextAsync(page.Text, chunkerConfig, conversationProviderConfig)).ToList();
                pageChunksMapping.Add((page, chunks));
            }));

            int totalChunks = pageChunksMapping.Sum(x => x.chunks.Count);
            int processedChunks = 0;
            var chunksToSaveBag = new ConcurrentBag<Chunk>();

            var channel = Channel.CreateUnbounded<EmbeddingProgressDTO>();
            _ = Task.Run(async () =>
            {
                var parallelTasks = pageChunksMapping.Select(async mapping =>
                {
                    var embeddingResults = await _embeddingProcessingService.GetEmbeddingsAsync(mapping.chunks, embeddingProviderConfig);
                    var batchToInsert = new List<(string, float[], Dictionary<string, string>)>();

                    foreach (var (chunk, vectorId, embedding) in embeddingResults)
                    {
                        batchToInsert.Add((vectorId, embedding, []));
                        chunksToSaveBag.Add(new Chunk { Text = chunk, VectorId = vectorId, PageId = mapping.page.Id });

                        int currentCount = Interlocked.Increment(ref processedChunks);
                        await channel.Writer.WriteAsync(new EmbeddingProgressDTO
                        {
                            ProcessedChunks = currentCount,
                            TotalChunks = totalChunks
                        }, cancellationToken);
                    }

                    await _embeddingProcessingService.InsertEmbeddingBatchAsync(batchToInsert, collectionId);
                });

                await Task.WhenAll(parallelTasks);
                await _embeddingProcessingService.AddChunksAsync(chunksToSaveBag.ToList());
                channel.Writer.Complete();
            }, cancellationToken);

            await foreach (var progress in channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return progress;
            }

            workflow.DocumentsCount++;
            await _workflowRepository.UpdateByApiKey(workflow, workflow.ApiKey);
        }
    }
}