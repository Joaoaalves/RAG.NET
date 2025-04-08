using RAGNET.Domain.Entities;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.Services
{
    public class EmbeddingProcessingService(
        IEmbedderFactory embedderFactory,
        ITextChunkerFactory chunkerFactory,
        IChatCompletionFactory chatCompletionFactory,
        IChunkRepository chunkRepository,
        IVectorDatabaseService vectorDatabaseService
    ) : IEmbeddingProcessingService
    {
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly ITextChunkerFactory _chunkerFactory = chunkerFactory;
        private readonly IChatCompletionFactory _chatCompletionFactory = chatCompletionFactory;
        private readonly IChunkRepository _chunkRepository = chunkRepository;
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;

        public async Task AddChunksAsync(List<Chunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                await _chunkRepository.AddAsync(new Chunk
                {
                    Text = chunk.Text,
                    VectorId = chunk.VectorId,
                    PageId = chunk.PageId
                });
            }

        }

        public Task<IEnumerable<string>> ChunkTextAsync(
            string text,
            Chunker chunkerConfig,
            ConversationProviderConfig conversationProviderConfig)
        {
            var completionService = _chatCompletionFactory.CreateCompletionService(conversationProviderConfig);
            var chunker = _chunkerFactory.CreateChunker(chunkerConfig, completionService);
            return chunker.ChunkText(text);
        }

        public async Task<List<(string ChunkText, string VectorId, float[] Embedding)>> GetEmbeddingsAsync(List<string> chunks, EmbeddingProviderConfig embeddingConfig)
        {
            var embedder = _embedderFactory.CreateEmbeddingService(embeddingConfig.ApiKey, embeddingConfig.Model, embeddingConfig.Provider);

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