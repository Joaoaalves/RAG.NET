using OpenAI.Embeddings;
using RAGNET.Application.Providers.Embedding;

namespace RAGNET.Infrastructure.Embedders
{
    public class OpenAIEmbeddingAdapter(string apiKey, string model) : IEmbeddingService
    {
        private readonly EmbeddingClient _embeddingClient = new(model, apiKey);

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(text);
            return embedding.ToFloats().ToArray();
        }

        public async Task<List<float[]>> GetMultipleEmbeddingAsync(List<string> texts)
        {
            var tasks = texts.Select(async chunk =>
            {
                return await GetEmbeddingAsync(chunk);
            });

            var embeddingsArr = await Task.WhenAll(tasks);
            return [.. embeddingsArr];
        }
    }
}
