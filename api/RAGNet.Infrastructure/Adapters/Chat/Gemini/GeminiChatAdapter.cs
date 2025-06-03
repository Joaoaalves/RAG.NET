using System.Text;
using System.Text.Json;
using RAGNET.Domain.Services;
using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Infrastructure.Exceptions.Adapters;

namespace RAGNET.Infrastructure.Adapters.Chat.Gemini
{
    public class GeminiChatAdapter : IChatCompletionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;

        public GeminiChatAdapter(string apiKey, string model = "gemini-2.0-flash")
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("API key must be provided", nameof(apiKey));

            _model = model;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://generativelanguage.googleapis.com/")
            };
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
        }

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            var url = $"v1beta/models/{_model}:generateContent";
            var payload = new
            {
                system_instruction = new { parts = new { text = systemPrompt } },
                contents = new[] { new { parts = new[] { new { text = message } } } }
            };

            var requestContent = JsonSerializer.Serialize(payload);
            try
            {
                using var response = await _httpClient.PostAsync(url,
                    new StringContent(requestContent, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GeminiChatException($"Gemini API error {response.StatusCode}: {body}");
                }

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
            catch (HttpRequestException he)
            {
                throw new GeminiChatException("HTTP request to Gemini API failed.", he);
            }
        }

        public async Task<JsonDocument> GetCompletionStructuredAsync(
            string systemPrompt,
            string message,
            JsonDocument jsonSchema,
            string? formatName)
        {
            var url = $"v1beta/models/{_model}:generateContent";

            var sanitizedSchema = SanitizeSchema(jsonSchema);

            var payload = new
            {
                system_instruction = new { parts = new { text = systemPrompt } },
                contents = new[] { new { parts = new[] { new { text = message } } } },
                generationConfig = new
                {
                    response_mime_type = "application/json",
                    response_schema = sanitizedSchema
                }
            };

            var requestContent = JsonSerializer.Serialize(payload);
            try
            {
                using var response = await _httpClient.PostAsync(url,
                    new StringContent(requestContent, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GeminiChatException($"Gemini API error {response.StatusCode}: {body}");
                }

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
            catch (HttpRequestException he)
            {
                throw new GeminiChatException("HTTP request to Gemini API failed.", he);
            }
        }

        private JsonElement SanitizeSchema(JsonDocument jsonSchema)
        {
            var raw = jsonSchema.RootElement.GetRawText();
            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(raw) ??
                       new Dictionary<string, JsonElement>();
            dict.Remove("additionalProperties");
            return JsonSerializer.SerializeToElement(dict);
        }
    }
}
