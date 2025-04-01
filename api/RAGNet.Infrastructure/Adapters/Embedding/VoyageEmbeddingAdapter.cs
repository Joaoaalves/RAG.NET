using System.Net.Http;
using System.Text;
using System.Text.Json;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Embedding
{
    public class VoyageEmbeddingAdapter(string apiKey, string model) : IEmbeddingService
    {
        private readonly string _voyageApiUrl = "https://api.voyageai.com/v1/embeddings";
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey = apiKey;
        private readonly string _model = model;

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _voyageApiUrl)
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

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            var embeddings = jsonDoc.RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding")
                .EnumerateArray()
                .Select(e => e.GetSingle())
                .ToArray();

            return embeddings;
        }

        public static List<EmbeddingModel> GetModels()
        {
            return
            [
                new() { Label = "Voyage 3 Large", Value = "voyage-3-large", Speed = 3, Price = 0.18f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage 3", Value = "voyage-3", Speed = 6, Price = 0.06f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage 3 Lite", Value = "voyage-3-lite", Speed = 7, Price = 0.02f, MaxContext = 32000, VectorSize = 512 },
                new() { Label = "Voyage Code 3", Value = "voyage-code-3", Speed = 4, Price = 0.18f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage Finance 2", Value = "voyage-finance-2", Speed = 4, Price = 0.12f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage Law 2", Value = "voyage-law-2", Speed = 4, Price = 0.12f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage Code 2", Value = "voyage-code-2", Speed = 4, Price = 0.12f, MaxContext = 32000, VectorSize = 1024 }
            ];
        }
    }
}