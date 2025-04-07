using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.AspNetCore.Http;
using RAGNET.Application.DTOs;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.EmbeddingUseCases
{
    public interface IProcessEmbeddingUseCase
    {
        Task<int> Execute(IFormFile file, string apiKey);
        IAsyncEnumerable<EmbeddingProgressDTO> ExecuteStreaming(
            IFormFile file,
            string apiKey,
            CancellationToken cancellationToken = default);
    }

    public class ProcessEmbeddingUseCase(
        IWorkflowRepository workflowRepository,
        IPdfTextExtractorService pdfTextExtractor,
        ITextChunkerFactory chunkerFactory,
        IEmbedderFactory embedderFactory,
        IVectorDatabaseService vectorDatabaseService,
        IChatCompletionFactory chatCompletionFactory
        ) : IProcessEmbeddingUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IPdfTextExtractorService _pdfTextExtractor = pdfTextExtractor;
        private readonly ITextChunkerFactory _chunkerFactory = chunkerFactory;
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IChatCompletionFactory _chatCompletionFactory = chatCompletionFactory;

        public async Task<int> Execute(IFormFile file, string apiKey)
        {

            var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey)
                           ?? throw new Exception("Workflow not found.");

            var chunkerConfig = workflow.Chunker
                                ?? throw new Exception("Chunker not found.");

            var embeddingProviderConfig = workflow.EmbeddingProviderConfig
                                ?? throw new Exception("Embedding Provider not set");

            var conversationProviderConfig = workflow.ConversationProviderConfig
                                ?? throw new Exception("Conversation provider config not set");

            // Creates the completion service for this workflow
            var completionService = _chatCompletionFactory.CreateCompletionService(conversationProviderConfig);

            // Creates the chunking Strategy for this workflow
            var chunker = _chunkerFactory.CreateChunker(chunkerConfig, completionService);

            // Creates the Embedder for this workflow
            var embedder = _embedderFactory.CreateEmbeddingService(embeddingProviderConfig.ApiKey, embeddingProviderConfig.Model, embeddingProviderConfig.Provider);

            // Get text from PDF
            var text = await _pdfTextExtractor.ExtractTextAsync(file);

            // Chunk
            var chunks = await chunker.ChunkText(text);

            var processedChunks = 0;
            var collectionId = workflow.CollectionId.ToString();

            // Embedd
            await Parallel.ForEachAsync(chunks, async (chunk, _) =>
            {
                var embedding = await embedder.GetEmbeddingAsync(chunk);

                var documentId = $"{workflow.Id}-{Interlocked.Increment(ref processedChunks)}";
                var metadata = new Dictionary<string, string>
                {
                    { "chunk", chunk }
                };

                await _vectorDatabaseService.InsertAsync(documentId, embedding, collectionId, metadata);
            });

            workflow.Documents++;

            await _workflowRepository.UpdateByApiKey(workflow, apiKey);

            return processedChunks;
        }

        public async IAsyncEnumerable<EmbeddingProgressDTO> ExecuteStreaming(
            IFormFile file,
            string apiKey,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey)
                           ?? throw new Exception("Workflow not found.");

            var chunkerConfig = workflow.Chunker
                                ?? throw new Exception("Chunker not found.");

            var embeddingProviderConfig = workflow.EmbeddingProviderConfig
                                ?? throw new Exception("Embedding Provider not set");

            var conversationProviderConfig = workflow.ConversationProviderConfig
                                ?? throw new Exception("Conversation provider config not set");

            var completionService = _chatCompletionFactory.CreateCompletionService(conversationProviderConfig);

            var chunker = _chunkerFactory.CreateChunker(chunkerConfig, completionService);
            var embedder = _embedderFactory.CreateEmbeddingService(embeddingProviderConfig.ApiKey, embeddingProviderConfig.Model, embeddingProviderConfig.Provider);

            var text = await _pdfTextExtractor.ExtractTextAsync(file);

            var chunks = await chunker.ChunkText(text);
            int totalChunks = chunks.Count();

            int processedChunks = 0;
            var collectionId = workflow.CollectionId.ToString();

            // Create a channel to stream progress updates.
            var channel = Channel.CreateUnbounded<EmbeddingProgressDTO>();

            // Start parallel processing of chunks.
            var processingTask = Task.Run(async () =>
            {
                await Parallel.ForEachAsync(chunks, cancellationToken, async (chunk, token) =>
                {
                    // Get the embedding for this chunk.
                    var embedding = await embedder.GetEmbeddingAsync(chunk);

                    // Create a document id and metadata.
                    var documentId = Guid.NewGuid().ToString();
                    var metadata = new Dictionary<string, string>
                    {
                    { "chunk", chunk }
                    };

                    // Insert the embedded chunk into the vector database.
                    await _vectorDatabaseService.InsertAsync(documentId, embedding, collectionId, metadata);

                    // Increment processed chunk count in a thread-safe manner.
                    int currentCount = Interlocked.Increment(ref processedChunks);

                    var progressUpdate = new EmbeddingProgressDTO
                    {
                        ProcessedChunks = currentCount,
                        TotalChunks = totalChunks
                    };

                    // Report progress by writing to the channel.
                    await channel.Writer.WriteAsync(progressUpdate, cancellationToken);
                });

                // Complete the channel when processing is done.
                channel.Writer.Complete();
            }, cancellationToken);

            // Yield progress updates as they become available.
            await foreach (var progress in channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return progress;
            }

            // Ensure all processing is complete.
            await processingTask;

            workflow.Documents++;

            await _workflowRepository.UpdateByApiKey(workflow, apiKey);
        }
    }
}
