using System.Text.Json;

using OpenAI.Chat;

using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.ChatCompletions.OpenAI
{
    public class OpenAIChatAdapter(
        IOpenAIChatClientWrapper chatClientWrapper,
        int delayMs = 2000
    ) : IConversationProviderService
    {
        private readonly IOpenAIChatClientWrapper _chatClientWrapper = chatClientWrapper;
        private readonly int _delayMs = delayMs;
        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            ChatMessage[] messages = [new SystemChatMessage(systemPrompt), new UserChatMessage(message)];

            return await RetryHelper.ExecuteWithRetryAsync(async () =>
            {
                return await _chatClientWrapper.CompleteChatAsync(messages);
            }, baseDelayMs: _delayMs);
        }

        public async Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName)
        {
            ChatMessage[] messages = [new SystemChatMessage(systemPrompt), new UserChatMessage(message)];

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
                var completion = await _chatClientWrapper.CompleteChatAsync(messages, options);
                return JsonDocument.Parse(completion);
            }, baseDelayMs: _delayMs);
        }
    }
}
