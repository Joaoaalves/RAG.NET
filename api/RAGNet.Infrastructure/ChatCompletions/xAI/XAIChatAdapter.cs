using System.Text.Json;
using OpenAI.Chat;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Infrastructure.SeedWork.Resilience;
using RAGNET.Infrastructure.ChatCompletions.OpenAI;

namespace RAGNET.Infrastructure.ChatCompletions.xAI
{
    public class XAIChatAdapter(
        IOpenAIChatClientWrapper clientWrapper,
        int delayMs = 2000
    ) : IConversationProviderService
    {
        private readonly IOpenAIChatClientWrapper _client = clientWrapper;
        private readonly int _delayMs = delayMs;

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            ChatMessage[] messages =
            [
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(message)
            ];

            return await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                return await _client.CompleteChatAsync(messages);
            }, baseDelayMs: _delayMs);
        }

        public async Task<JsonDocument> GetCompletionStructuredAsync(
            string systemPrompt,
            string message,
            JsonDocument jsonSchema,
            string? formatName)
        {
            ChatMessage[] messages =
            [
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(message)
            ];

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: formatName!,
                    jsonSchema: BinaryData.FromObjectAsJson(jsonSchema),
                    jsonSchemaIsStrict: true
                )
            };

            return await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                try
                {
                    var result = await _client.CompleteChatAsync(messages, options);
                    return JsonDocument.Parse(result);
                }
                catch (JsonException exc)
                {
                    throw new JsonException("Failed to parse JSON for XAIChatAdapter", exc);
                }
            }, baseDelayMs: _delayMs);
        }
    }
}
