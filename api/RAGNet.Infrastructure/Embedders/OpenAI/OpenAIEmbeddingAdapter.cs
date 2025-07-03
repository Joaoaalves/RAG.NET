using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.Embedders.OpenAI
{
    public class OpenAIEmbeddingAdapter(
        IOpenAIEmbeddingWrapper embeddingClient,
        int delayMs = 2000
    ) : IEmbeddingService
    {
        private readonly IOpenAIEmbeddingWrapper _client = embeddingClient;
        private readonly int _delayMs = delayMs;
        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            return await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                return await _client.GenerateEmbeddingAsync(text);
            }, baseDelayMs: _delayMs);
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
