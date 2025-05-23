using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Exceptions.Adapters;

namespace RAGNET.Infrastructure.Adapters.Chat
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

        public static List<ConversationModel> GetModels()
        {
            return
            [
                new ConversationModel
                {
                    Label = "Gemini 2.5 Flash Preview",
                    Value = "gemini-2.5-flash-preview-04-17",
                    Speed = 3,
                    InputPrice = 0.15f,
                    OutputPrice = 0.6f,
                    MaxOutput = 65536,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 2.5 Pro Preview",
                    Value = "gemini-2.5-pro-preview-03-25",
                    Speed = 2,
                    InputPrice = 2.5f,
                    OutputPrice = 15,
                    MaxOutput = 65536,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 2.0 Flash",
                    Value = "gemini-2.0-flash",
                    Speed = 5,
                    InputPrice = 0.1f,
                    OutputPrice = 0.4f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 2.0 Flash-Lite",
                    Value = "gemini-2.0-flash-lite",
                    Speed = 5,
                    InputPrice = 0.075f,
                    OutputPrice = 0.3f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 1.5 Flash",
                    Value = "gemini-1.5-flash",
                    Speed = 4,
                    InputPrice = 0.15f,
                    OutputPrice = 0.6f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 1.5 Flash-8B",
                    Value = "gemini-1.5-flash-8b",
                    Speed = 5,
                    InputPrice = 0.075f,
                    OutputPrice = 0.3f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 1.5 Pro",
                    Value = "gemini-1.5-pro",
                    Speed = 4,
                    InputPrice = 2.5f,
                    OutputPrice = 10,
                    MaxOutput = 8192,
                    ContextWindow = 2_097_152
                }
            ];
        }
    }
}
