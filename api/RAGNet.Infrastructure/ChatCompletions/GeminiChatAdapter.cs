using System.Text;
using System.Text.Json;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Infrastructure.Exceptions.Adapters;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.ChatCompletions
{
    public class GeminiChatAdapter : IConversationProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly int _delayMs;

        public GeminiChatAdapter(string apiKey, string model = "gemini-2.0-flash", HttpClient? httpClient = null, int delayMs = 2000)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("API key must be provided", nameof(apiKey));

            _model = model;
            _httpClient = httpClient ?? new HttpClient
            {
                BaseAddress = new Uri("https://generativelanguage.googleapis.com/")
            };
            if (!_httpClient.DefaultRequestHeaders.Contains("x-goog-api-key"))
                _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

            _delayMs = delayMs;
        }

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            var url = $"v1beta/models/{_model}:generateContent";

            var result = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var request = BuildRequest(url, systemPrompt, message);
                using var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new GeminiChatException($"Gemini API error {response.StatusCode}: {body}");

                try
                {
                    using var doc = JsonDocument.Parse(body);
                    var text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return text!;
                }
                catch (JsonException je)
                {
                    throw new GeminiChatException("Failed to parse JSON response from Gemini.", je);
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
            var url = $"v1beta/models/{_model}:generateContent";
            var sanitizedSchema = SanitizeSchema(jsonSchema);

            var result = await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                var request = BuildRequest(url, systemPrompt, message, sanitizedSchema);
                using var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new GeminiChatException($"Gemini API error {response.StatusCode}: {body}");

                try
                {
                    using var wrapper = JsonDocument.Parse(body);
                    var jsonText = wrapper.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return JsonDocument.Parse(jsonText!);
                }
                catch (JsonException je)
                {
                    throw new GeminiChatException("Failed to parse structured JSON from Gemini.", je);
                }
            }, baseDelayMs: _delayMs);

            return result;
        }

        private static HttpRequestMessage BuildRequest(string url, string systemPrompt, string message, JsonElement? sanitizedSchema = null)
        {
            object payload = sanitizedSchema.HasValue
                ? new
                {
                    system_instruction = new { parts = new { text = systemPrompt } },
                    contents = new[] { new { parts = new[] { new { text = message } } } },
                    generationConfig = new
                    {
                        response_mime_type = "application/json",
                        response_schema = sanitizedSchema.Value
                    }
                }
                : new
                {
                    system_instruction = new { parts = new { text = systemPrompt } },
                    contents = new[] { new { parts = new[] { new { text = message } } } }
                };

            var requestContent = JsonSerializer.Serialize(payload);

            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };
        }

        private static JsonElement SanitizeSchema(JsonDocument jsonSchema)
        {
            var raw = jsonSchema.RootElement.GetRawText();
            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(raw) ??
                       [];
            dict.Remove("additionalProperties");
            return JsonSerializer.SerializeToElement(dict);
        }
    }
}
