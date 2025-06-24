using RAGNET.Application.Chunkers.Factories;
using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Documents.Pages.Chunks;

using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers.Embedding
{
    public interface IEmbeddingProcessingService
    {
        Task<IEnumerable<string>> ChunkTextAsync
        (
            ITextChunkerService chunker,
            string text
        );
        ITextChunkerService GetChunker(
            Chunker chunkerConfig,
            ConversationProviderConfig conversationProviderConfig,
            string userConversationProviderApiKey
        );
        Task<List<(string ChunkText, string VectorId, float[] Embedding)>> GetEmbeddingsAsync
        (
            List<string> chunks,
            EmbeddingProviderConfig embeddingConfig,
            string userEmbeddingProviderApiKey
        );
        Task AddChunksAsync(List<Chunk> chunks);
        Task InsertEmbeddingBatchAsync(List<(string VectorId, float[] Embedding, Dictionary<string, string> Metadata)> batch, string collectionId);
    }

    public class EmbeddingProcessingService(
        IEmbedderFactory embedderFactory,
        ITextChunkerFactory chunkerFactory,
        IConversationProviderFactory chatCompletionFactory,
        IChunkRepository chunkRepository,
        IVectorDatabaseService vectorDatabaseService
    ) : IEmbeddingProcessingService
    {
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly ITextChunkerFactory _chunkerFactory = chunkerFactory;
        private readonly IConversationProviderFactory _chatCompletionFactory = chatCompletionFactory;
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
            ConversationProviderConfig conversationProviderConfig,
            string userConversationProviderApiKey
        )
        {
            var completionService = _chatCompletionFactory.CreateCompletionService
            (
                userConversationProviderApiKey,
                conversationProviderConfig
            );

            return _chunkerFactory.CreateChunker(chunkerConfig, completionService);
        }

        public Task<IEnumerable<string>> ChunkTextAsync(
            ITextChunkerService chunker,
            string text
        )
        {
            return chunker.ChunkText(text);
        }

        public async Task<List<(string ChunkText, string VectorId, float[] Embedding)>> GetEmbeddingsAsync(
            List<string> chunks,
            EmbeddingProviderConfig embeddingConfig,
            string userEmbeddingProviderApiKey
        )
        {
            var embedder = _embedderFactory.CreateEmbeddingService
            (
                userEmbeddingProviderApiKey,
                embeddingConfig
            );

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