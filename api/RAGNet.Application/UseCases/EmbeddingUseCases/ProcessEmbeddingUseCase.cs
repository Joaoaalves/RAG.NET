using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.EmbeddingUseCases
{
    public interface IProcessEmbeddingUseCase
    {
        Task<int> Execute(IFormFile file, string apiKey);
    }

    public class ProcessEmbeddingUseCase(
        IWorkflowRepository workflowRepository,
        IPdfTextExtractorService pdfTextExtractor,
        ITextChunkerFactory chunkerFactory,
        IEmbedderFactory embedderFactory,
        IVectorDatabaseService vectorDatabaseService) : IProcessEmbeddingUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IPdfTextExtractorService _pdfTextExtractor = pdfTextExtractor;
        private readonly ITextChunkerFactory _chunkerFactory = chunkerFactory;
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;

        public async Task<int> Execute(IFormFile file, string apiKey)
        {

            var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey)
                           ?? throw new Exception("Workflow not found.");

            var chunkerConfig = workflow.Chunker
                                ?? throw new Exception("Chunker not found.");

            var embeddingProviderConfig = workflow.EmbeddingProviderConfig
                                ?? throw new Exception("Embedding Provider not set");

            // Creates the chunking Strategy for this workflow
            var chunker = _chunkerFactory.CreateChunker(chunkerConfig);

            // Creates the Embedder for this workflow
            var embedder = _embedderFactory.CreateEmbeddingService(embeddingProviderConfig.ApiKey);

            // Get text from PDF
            var text = await _pdfTextExtractor.ExtractTextAsync(file);

            // Chunk
            var chunks = chunker.ChunkText(text);

            var processedChunks = 0;
            var collectionId = workflow.CollectionId.ToString();

            // Embedd
            await Parallel.ForEachAsync(chunks, async (chunk, _) =>
            {
                var embedding = await embedder.GetEmbeddingAsync(chunk);

                var documentId = $"{workflow.Id}-{Interlocked.Increment(ref processedChunks)}";
                var metadata = new Dictionary<string, string>
                {
                    { "chunkPreview", chunk.Length > 100 ? chunk.Substring(0, 100) : chunk }
                };

                await _vectorDatabaseService.InsertAsync(documentId, embedding, collectionId, metadata);
            });

            return processedChunks;
        }
    }
}
