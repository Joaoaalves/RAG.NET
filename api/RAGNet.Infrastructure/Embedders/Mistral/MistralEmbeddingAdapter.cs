using System.Text;
using System.Text.Json;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.Embedders.Mistral
{
    public class MistralEmbeddingAdapter : IEmbeddingService
    {
        private readonly string _model;
        private readonly HttpClient _httpClient;
        private readonly int _delayMs;

        public MistralEmbeddingAdapter(string apiKey, string model = "mistral-embed", HttpClient? httpClient = null, int delayMs = 2000)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key must be provided", nameof(apiKey));

            _model = model;
            _httpClient = httpClient ?? new HttpClient
            {
                BaseAddress = new Uri("https://api.mistral.ai/")
            };

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            _delayMs = delayMs;
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var response = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var request = BuildRequest([text]);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response;
            }, baseDelayMs: _delayMs);

            return await ParseSingleEmbedding(response);
        }

        public async Task<List<float[]>> GetMultipleEmbeddingAsync(List<string> texts)
        {
            var response = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var request = BuildRequest(texts);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response;
            }, baseDelayMs: _delayMs);

            return await ParseBatchEmbeddings(response);
        }

        private HttpRequestMessage BuildRequest(List<string> texts)
        {
            var url = "v1/embeddings";

            var payload = new
            {
                model = _model,
                input = texts
            };

            var json = JsonSerializer.Serialize(payload);

            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static async Task<float[]> ParseSingleEmbedding(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(body);
                var vectorElement = doc.RootElement
                    .GetProperty("data")[0]
                    .GetProperty("embedding");

                return ParseFloatArray(vectorElement);
            }
            catch (JsonException je)
            {
                throw new JsonException("Failed to parse single embedding from Mistral response.", je);
            }
        }

        private static async Task<List<float[]>> ParseBatchEmbeddings(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(body);
                var embeddings = doc.RootElement.GetProperty("data");

                var list = new List<float[]>();
                foreach (var item in embeddings.EnumerateArray())
                {
                    list.Add(ParseFloatArray(item.GetProperty("embedding")));
                }

                return list;
            }
            catch (JsonException je)
            {
                throw new JsonException("Failed to parse batch embeddings from Mistral response.", je);
            }
        }

        private static float[] ParseFloatArray(JsonElement vectorElement)
        {
            var vector = new float[vectorElement.GetArrayLength()];
            int i = 0;
            foreach (var value in vectorElement.EnumerateArray())
            {
                vector[i++] = value.GetSingle();
            }
            return vector;
        }
    }
}
