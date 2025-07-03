using OpenAI.Embeddings;

namespace RAGNET.Infrastructure.Embedders.OpenAI
{
    public class OpenAIEmbeddingClientWrapper(
        string apiKey, string model
    ) : IOpenAIEmbeddingWrapper
    {
        private readonly EmbeddingClient _client = new(model, apiKey);
        public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            OpenAIEmbedding embedding = await _client.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);
            return embedding.ToFloats().ToArray();
        }
    }
}