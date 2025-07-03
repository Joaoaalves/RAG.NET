using System.Text;
using System.Text.Json;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.ChatCompletions.Mistral
{
    public class MistralChatAdapter : IConversationProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly int _delayMs;
        private readonly Uri _baseAddress = new("https://api.mistral.ai/");
        private readonly string CompletionEndpoint = "v1/chat/completions";

        public MistralChatAdapter(string apiKey, string model = "magistral-small-2506", HttpClient? httpClient = null, int delayMs = 2000)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("API key must be provided", nameof(apiKey));

            _model = model;
            _httpClient = httpClient ?? new HttpClient
            {
                BaseAddress = _baseAddress
            };

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            _delayMs = delayMs;
        }

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            var result = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var request = BuildRequest(systemPrompt, message);
                using var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Mistral API error {response.StatusCode}: {body}");

                try
                {
                    using var doc = JsonDocument.Parse(body);
                    return doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString()!;
                }
                catch (JsonException je)
                {
                    throw new JsonException("Failed to parse JSON response from Mistral.", je);
                }
            }, baseDelayMs: _delayMs);

            return result;
        }

        public async Task<JsonDocument> GetCompletionStructuredAsync(
            string systemPrompt,
            string message,
            JsonDocument jsonSchema,
            string? formatName)
        {

            var result = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var request = BuildRequest(systemPrompt, message, jsonSchema, formatName);
                using var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Mistral API error {response.StatusCode}: {body}");

                try
                {
                    using var wrapper = JsonDocument.Parse(body);
                    var jsonText = wrapper.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return JsonDocument.Parse(jsonText!);
                }
                catch (JsonException je)
                {
                    throw new JsonException("Failed to parse structured JSON from Mistral.", je);
                }
            }, baseDelayMs: _delayMs);

            return result;
        }

        private HttpRequestMessage BuildRequest(
            string systemPrompt,
            string userMessage,
            JsonDocument? jsonSchema = null,
            string? formatName = null)
        {
            var messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userMessage }
            };

            object payload = jsonSchema is null
                ? new
                {
                    model = _model,
                    messages,
                    temperature = 0,
                    max_tokens = 512
                }
                : new
                {
                    model = _model,
                    messages,
                    temperature = 0,
                    max_tokens = 512,
                    response_format = new
                    {
                        type = "json_schema",
                        json_schema = new
                        {
                            name = formatName ?? "schema",
                            strict = true,
                            schema = jsonSchema.RootElement
                        }
                    }
                };

            var json = JsonSerializer.Serialize(payload);

            return new HttpRequestMessage(HttpMethod.Post, CompletionEndpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}
