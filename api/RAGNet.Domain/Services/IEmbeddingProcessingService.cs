using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IEmbeddingProcessingService
    {
        Task<IEnumerable<string>> ChunkTextAsync(string text, Chunker chunkerConfig, ConversationProviderConfig conversationProviderConfig);
        Task<List<(string ChunkText, string VectorId, float[] Embedding)>> GetEmbeddingsAsync(List<string> chunks, EmbeddingProviderConfig embeddingConfig);
        Task AddChunksAsync(List<Chunk> chunks);
        Task InsertEmbeddingBatchAsync(List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)> batch, string collectionId);
    }
}