using OpenAI.Embeddings;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.Embedders
{
    public class OpenAIEmbeddingAdapter : IEmbeddingService
    {
        private readonly EmbeddingClient _embeddingClient;

        private OpenAIEmbeddingAdapter(EmbeddingClient embeddingClient)
        {
            _embeddingClient = embeddingClient;
        }

        public static OpenAIEmbeddingAdapter FromClient(EmbeddingClient client)
        {
            return new OpenAIEmbeddingAdapter(client);
        }

        public static OpenAIEmbeddingAdapter FromApiKey(string apiKey, string model)
        {
            var client = new EmbeddingClient(model, apiKey);
            return new OpenAIEmbeddingAdapter(client);
        }


        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            return await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(text);
                return embedding.ToFloats().ToArray();
            });
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
