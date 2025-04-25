using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.AspNetCore.Http;
using RAGNET.Application.DTOs;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Application.UseCases.EmbeddingUseCases
{
    public interface IProcessEmbeddingUseCase
    {
        Task<int> Execute(IFormFile file, Workflow workflow);
        IAsyncEnumerable<EmbeddingProgressDTO> ExecuteStreaming(IFormFile file, Workflow workflow, CancellationToken cancellationToken = default);
    }

    public class ProcessEmbeddingUseCase(
        IWorkflowRepository workflowRepository,
        IDocumentProcessorFactory documentProcessorFactory,
        IEmbeddingProcessingService embeddingProcessingService,
        IJobStatusRepository jobStatusRepository,
        IApiKeyResolverService apiKeyResolverService) : IProcessEmbeddingUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IDocumentProcessorFactory _documentProcessorFactory = documentProcessorFactory;
        private readonly IEmbeddingProcessingService _embeddingProcessingService = embeddingProcessingService;
        private readonly IApiKeyResolverService _apiKeyResolverService = apiKeyResolverService;
        private readonly IJobStatusRepository _jobStatusRepository = jobStatusRepository;

        private (string collectionId, Chunker chunkerConfig, EmbeddingProviderConfig embeddingProviderConfig, ConversationProviderConfig conversationProviderConfig) GetConfig(Workflow workflow)
        {
            return (
                workflow.CollectionId.ToString(),
                workflow.Chunker ?? throw new Exception("Chunker not found."),
                workflow.EmbeddingProviderConfig ?? throw new Exception("Embedding Provider not set."),
                workflow.ConversationProviderConfig ?? throw new Exception("Conversation provider config not set.")
            );
        }

        private async Task<Document> ProcessDocumentAsync(IFormFile file, Workflow workflow)
        {
            var documentExtension = GetFileExtension(file.FileName);
            var documentProcessingAdapter = _documentProcessorFactory.CreateDocumentProcessor(documentExtension);

            var ms = file.OpenReadStream();
            var pdfExtractResult = await documentProcessingAdapter.ExtractTextAsync(ms);

            return await documentProcessingAdapter.CreateDocumentWithPagesAsync(
                file.FileName.Replace(documentExtension, ""),
                workflow.Id,
                pdfExtractResult.Pages
            );
        }

        private string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).ToLowerInvariant();
        }

        private async Task<string> GetConversationProviderApiKey(Workflow workflow, ConversationProviderConfig conversationProviderConfig)
        {

            return await _apiKeyResolverService.ResolveForUserAsync(workflow.UserId, conversationProviderConfig.Provider.ToSupportedProvider());
        }

        private async Task<string> GetEmbeddingProviderApiKey(Workflow workflow, EmbeddingProviderConfig embeddingProviderConfig)
        {
            return await _apiKeyResolverService.ResolveForUserAsync(workflow.UserId, embeddingProviderConfig.Provider.ToSupportedProvider());
        }

        private async Task<Guid> EnqueueJob(IFormFile file, Workflow workflow)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var job = new EmbeddingJob
            {
                ApiKey = workflow.ApiKey,
                UserId = workflow.UserId,
                FileName = file.FileName,
                FileContent = ms.ToArray(),
                CallbackUrls = [.. workflow.CallbackUrls]
            };

            await _jobStatusRepository.SetPendingAsync(job.JobId);

            return job.JobId;
        }

        private async Task CompleteJob(Guid jobId)
        {
            await _jobStatusRepository.MarkAsCompletedAsync(jobId);
        }

        public async Task<int> Execute(IFormFile file, Workflow workflow)
        {
            // Enque
            var jobId = await EnqueueJob(file, workflow);

            var (collectionId, chunkerConfig, embeddingProviderConfig, conversationProviderConfig) = GetConfig(workflow);

            var document = await ProcessDocumentAsync(file, workflow);

            var chunksToSaveBag = new ConcurrentBag<Chunk>();

            var tasks = document.Pages.Select(async page =>
            {
                var chunks = (await _embeddingProcessingService.ChunkTextAsync
                (
                    page.Text,
                    chunkerConfig,
                    conversationProviderConfig,
                    await GetConversationProviderApiKey(workflow, conversationProviderConfig)
                )).ToList();

                var embeddingResults = await _embeddingProcessingService.GetEmbeddingsAsync
                (
                    chunks,
                    embeddingProviderConfig,
                    await GetEmbeddingProviderApiKey(workflow, embeddingProviderConfig)
                );

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

            await CompleteJob(jobId);
            return processedChunks;
        }

        public async IAsyncEnumerable<EmbeddingProgressDTO> ExecuteStreaming(IFormFile file, Workflow workflow, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var (collectionId, chunkerConfig, embeddingProviderConfig, conversationProviderConfig) = GetConfig(workflow);
            var document = await ProcessDocumentAsync(file, workflow);

            var pageChunksMapping = new ConcurrentBag<(Page page, List<string> chunks)>();
            await Task.WhenAll(document.Pages.Select(async page =>
            {
                var chunks = (await _embeddingProcessingService.ChunkTextAsync
                (
                    page.Text,
                    chunkerConfig,
                    conversationProviderConfig,
                    await GetConversationProviderApiKey(workflow, conversationProviderConfig
                    ))).ToList();
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
                    var embeddingResults = await _embeddingProcessingService.GetEmbeddingsAsync(
                        mapping.chunks,
                        embeddingProviderConfig,
                        await GetEmbeddingProviderApiKey(workflow, embeddingProviderConfig)
                    );

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