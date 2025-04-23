using System.Text;
using System.Text.Json;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Exceptions.Adapters;

namespace RAGNET.Infrastructure.Adapters.Embedding
{
    public class GeminiEmbeddingAdapter : IEmbeddingService
    {
        private readonly string _model;
        private readonly HttpClient _httpClient;

        public GeminiEmbeddingAdapter(string apiKey, string model = "gemini-embedding-exp-03-07")
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("API Key must be provided", nameof(apiKey));

            _model = model;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://generativelanguage.googleapis.com/")
            };
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
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

            var requestContent = JsonSerializer.Serialize(payload);
            try
            {
                using var response = await _httpClient.PostAsync(
                    url,
                    new StringContent(requestContent, Encoding.UTF8, "application/json")
                ).ConfigureAwait(false);

                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new GeminiEmbeddingException($"Gemini API error {response.StatusCode}: {body}");
                }

                using var doc = JsonDocument.Parse(body);

                var vectorElement = doc.RootElement.GetProperty("embedding").GetProperty("values");
                return ParseFloatArray(vectorElement);
            }
            catch (JsonException je)
            {
                throw new GeminiEmbeddingException("Failed to parse JSON response from Gemini.", je);
            }
            catch (HttpRequestException he)
            {
                throw new GeminiEmbeddingException("HTTP request to Gemini API failed.", he);
            }
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

        public static List<EmbeddingModel> GetModels()
        {
            return [
                new() {
                    Label = "Gemini Embedding Experimental",
                    Value = "gemini-embedding-exp-03-07",
                    Speed = 4,
                    Price = 0,
                    MaxContext = 8192,
                    VectorSize = 3072
                },
                new() {
                    Label = "Text Embedding",
                    Value = "text-embedding-004",
                    Speed = 4,
                    Price = 0,
                    MaxContext = 8192,
                    VectorSize = 768
                },
            ];
        }
    }
}