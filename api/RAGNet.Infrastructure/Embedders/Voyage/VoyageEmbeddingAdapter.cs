using System.Text;
using System.Text.Json;

using RAGNET.Domain.SharedKernel.VectorStorages.SemanticVectors;

using RAGNET.Application.Infrastructure.Providers.Embedding;

using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.Embedders.Voyage
{
    public class VoyageEmbeddingAdapter : IEmbeddingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly int _delayMs;

        public VoyageEmbeddingAdapter(string apiKey, string model, HttpClient? httpClient = null, int delayMs = 2000)
        {
            _model = model;
            _apiKey = apiKey;
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.voyageai.com/v1/");
            _delayMs = delayMs;
        }

        public async Task<SemanticVector> GetEmbeddingAsync(string text)
        {

            var response = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var request = BuildRequest(text);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response;
            }, baseDelayMs: _delayMs);

            return await ParseBody(response);
        }

        public async Task<List<SemanticVector>> GetMultipleEmbeddingAsync(List<string> texts)
        {
            var tasks = texts.Select(GetEmbeddingAsync);
            var embeddingsArr = await Task.WhenAll(tasks);
            return [.. embeddingsArr];
        }

        private static async Task<SemanticVector> ParseBody(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            var vector = jsonDoc.RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding")
                .EnumerateArray()
                .Select(e => e.GetSingle())
                .ToArray();

            return new SemanticVector(vector);
        }

        private HttpRequestMessage BuildRequest(string text)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "embeddings")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        input = new[] { text },
                        model = _model
                    }),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            return request;
        }
    }
}
