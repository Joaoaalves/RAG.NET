using OpenAI.Embeddings;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Embedding
{
    public class OpenAIEmbeddingAdapter(string apiKey, string model) : IEmbeddingService
    {
        private readonly EmbeddingClient _embeddingClient = new(model, apiKey);

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(text);
            return embedding.ToFloats().ToArray();
        }

        public static List<EmbeddingModel> GetModels()
        {
            return [
                new() { Label = "Embedding 3 Large", Value = "text-embedding-3-large", Speed = 4, Price = 0.13f, MaxContext = 8191, VectorSize = 1536 },
                new() { Label = "Embedding 3 Small", Value = "text-embedding-3-small", Speed = 5, Price = 0.02f, MaxContext = 8191, VectorSize = 1536 },
                new() { Label = "Embedding Ada 002", Value = "text-embedding-ada-002", Speed = 3, Price = 0.10f, MaxContext = 8191, VectorSize = 1536 },
            ];
        }
    }
}
