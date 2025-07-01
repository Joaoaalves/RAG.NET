using RAGNET.Application.Chunkers.Factories;
using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Domain.Chunkers;
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
        ITextChunkerService GetChunker(
            Chunker chunkerConfig,
            IConversationProviderService completionService
        );
        Task<List<(string ChunkText, string VectorId, float[] Embedding)>> GetEmbeddingsAsync
        (
            List<string> chunks,
            IEmbeddingService embedder
        );
        Task AddChunksAsync(List<Chunk> chunks);
        Task InsertEmbeddingBatchAsync(List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)> batch, string collectionId);
    }

    public class EmbeddingProcessingService(
        ITextChunkerFactory chunkerFactory,
        IChunkRepository chunkRepository,
        IVectorDatabaseService vectorDatabaseService
    ) : IEmbeddingProcessingService
    {
        private readonly ITextChunkerFactory _chunkerFactory = chunkerFactory;
        private readonly IChunkRepository _chunkRepository = chunkRepository;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;

        public async Task AddChunksAsync(List<Chunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                await _chunkRepository.AddAsync(chunk);
            }

        }

        public ITextChunkerService GetChunker(
            Chunker chunkerConfig,
            IConversationProviderService completionService
        )
        {
            return _chunkerFactory.CreateChunker(chunkerConfig, completionService);
        }

        public Task<List<string>> ChunkTextAsync(
            ITextChunkerService chunker,
            string text
        )
        {
            return chunker.ChunkText(text);
        }

        public async Task<List<(string ChunkText, string VectorId, float[] Embedding)>> GetEmbeddingsAsync(
            List<string> chunks,
            IEmbeddingService embedder
        )
        {
            var embeddings = await embedder.GetMultipleEmbeddingAsync(chunks);
            var result = new List<(string, string, float[])>();

            for (int i = 0; i < chunks.Count; i++)
            {
                string vectorId = Guid.NewGuid().ToString();
                result.Add((chunks[i], vectorId, embeddings[i]));
            }

            return result;
        }

        public async Task InsertEmbeddingBatchAsync(List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)> batch, string collectionId)
        {
            await _vectorDatabaseService.InsertManyAsync(batch, collectionId);
        }
    }
}