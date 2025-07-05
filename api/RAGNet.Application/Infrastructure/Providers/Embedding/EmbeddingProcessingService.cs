using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Application.Infrastructure.Providers.Embedding
{
    public interface IEmbeddingProcessingService
    {
        Task<List<string>> ChunkTextAsync
        (
            ITextChunkerService chunker,
            string text
        );
        Task<List<EmbeddingDTO>> GetEmbeddingsAsync
        (
            List<string> chunks,
            IEmbeddingService embedder
        );
        Task InsertEmbeddingBatchAsync(List<EmbeddingDTO> batch, string collectionId);
    }

    public class EmbeddingProcessingService(
        IVectorDatabaseService vectorDatabaseService
    ) : IEmbeddingProcessingService
    {
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;

        public Task<List<string>> ChunkTextAsync(
            ITextChunkerService chunker,
            string text
        )
        {
            return chunker.ChunkText(text);
        }

        public async Task<List<EmbeddingDTO>> GetEmbeddingsAsync(
            List<string> chunks,
            IEmbeddingService embedder
        )
        {
            var vectors = await embedder.GetMultipleEmbeddingAsync(chunks);
            var result = new List<EmbeddingDTO>();

            for (int i = 0; i < chunks.Count; i++)
            {
                string vectorId = Guid.NewGuid().ToString();

                result.Add(new()
                {
                    VectorId = vectorId,
                    Vector = vectors[i],
                    ChunkText = chunks[i],
                });
            }

            return result;
        }

        public async Task InsertEmbeddingBatchAsync(List<EmbeddingDTO> batch, string collectionId)
        {
            await _vectorDatabaseService.InsertManyAsync(batch, collectionId);
        }
    }
}