using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;

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
    }

    public class EmbeddingProcessingService : IEmbeddingProcessingService
    {

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
    }
}