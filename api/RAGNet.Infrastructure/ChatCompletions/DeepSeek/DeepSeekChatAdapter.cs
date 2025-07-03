using System.Text.Json;

using DeepSeek.Core.Models;

using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.ChatCompletions.DeepSeek
{
    public class DeepSeekChatAdapter(
        IDeepSeekClientWrapper client,
        int delayMs = 2000
    ) : IConversationProviderService
    {
        private readonly IDeepSeekClientWrapper _client = client;
        private readonly int _delayMs = delayMs;

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            var messages = BuildMessages(systemPrompt, message);

            return await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                return await _client.CompleteChatAsync(messages);
            }, baseDelayMs: _delayMs);
        }

        public async Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName = null)
        {
            var jsonInstruction = BuildJsonFormat(jsonSchema);

            var messages = BuildMessages(systemPrompt, message, jsonInstruction);

            ResponseFormat responseFormat = new()
            {
                Type = "json_object"
            };
            Console.WriteLine("Starting call");
            return await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                try
                {
                    Console.WriteLine("Calling");
                    var completion = await _client.CompleteChatAsync(messages, responseFormat);
                    return JsonDocument.Parse(completion);
                }
                catch (JsonException exc)
                {
                    throw new InvalidOperationException("Error while parsing JSON", exc);
                }
            }, baseDelayMs: _delayMs);
        }

        private static List<Message> BuildMessages(string systemPrompt, string message, string? toolPrompt = null)
        {
            var messages = new List<Message>();

            if (toolPrompt is not null)
                messages.Add(Message.NewSystemMessage(toolPrompt));

            messages.Add(Message.NewSystemMessage(systemPrompt));
            messages.Add(Message.NewUserMessage(message));

            return messages;
        }

        private static string BuildJsonFormat(JsonDocument schema)
        {
            string schemaJson = JsonSerializer.Serialize(schema.RootElement);
            return $"YOU SHOULD ANSWER ONLY JSON. YOUR ANSWER SHOULD FIT TO THIS SCHEMA:\n\n{schemaJson}";
        }
    }
}