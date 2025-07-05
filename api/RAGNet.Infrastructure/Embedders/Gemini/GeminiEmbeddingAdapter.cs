using System.Text;
using System.Text.Json;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Infrastructure.Exceptions.Adapters;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.Embedders.Gemini
{
    public class GeminiEmbeddingAdapter : IEmbeddingService
    {
        private readonly string _model;
        private readonly HttpClient _httpClient;
        private readonly int _delayMs;

        public GeminiEmbeddingAdapter(string apiKey, string model, HttpClient? httpClient = null, int delayMs = 2000)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("API Key must be provided", nameof(apiKey));

            _model = model;
            _httpClient = httpClient ?? new HttpClient
            {
                BaseAddress = new Uri("https://generativelanguage.googleapis.com/")
            }; ;
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
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

        private HttpRequestMessage BuildRequest(string text)
        {
            var url = $"v1beta/models/{_model}:embedContent";

            var payload = new
            {
                content = new
                {
                    parts = new[]
                    {
                        new Dictionary<string, string>
                        {
                            { "text", text }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static async Task<SemanticVector> ParseBody(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(body);
                var vectorElement = doc.RootElement
                    .GetProperty("embedding")
                    .GetProperty("values");

                return ParseSemanticVector(vectorElement);
            }
            catch (JsonException je)
            {
                throw new GeminiEmbeddingException("Failed to parse JSON response from Gemini.", je);
            }
        }

        private static SemanticVector ParseSemanticVector(JsonElement vectorElement)
        {
            var vector = new float[vectorElement.GetArrayLength()];
            int i = 0;

            foreach (var value in vectorElement.EnumerateArray())
            {
                vector[i++] = value.GetSingle();
            }

            return new SemanticVector(vector);
        }
    }
}
