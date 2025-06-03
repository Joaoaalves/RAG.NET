using System.Text;
using System.Text.Json;

using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Embedding.Voyage
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