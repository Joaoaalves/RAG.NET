using OpenAI.Embeddings;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Embedding
{
    public class OpenAIEmbeddingAdapter : IEmbeddingService
    {
        private readonly EmbeddingClient _embeddingClient;
        public OpenAIEmbeddingAdapter(string apiKey, string model)
        {
            _embeddingClient = new(model, apiKey);
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(text);
            return embedding.ToFloats().ToArray();
        }
    }
}
