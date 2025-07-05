using OpenAI.Embeddings;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Infrastructure.Embedders.OpenAI
{
    public class OpenAIEmbeddingClientWrapper(string apiKey, string model) : IOpenAIEmbeddingWrapper
    {
        private readonly EmbeddingClient _client = new(model, apiKey);

        public async Task<SemanticVector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
        {
            OpenAIEmbedding embedding = await _client.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);
            var vector = new SemanticVector(embedding.ToFloats().ToArray());

            return vector;
        }
    }
}