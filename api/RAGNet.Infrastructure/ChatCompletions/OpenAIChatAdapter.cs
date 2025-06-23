using System.Text.Json;
using OpenAI.Chat;

using RAGNET.Application.Infrastructure.Providers.Conversation;

namespace RAGNET.Infrastructure.ChatCompletions
{
    public class OpenAIChatAdapter(string apiKey, string model) : IConversationProviderService
    {
        private readonly ChatClient _chatClient = new(model, apiKey);

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            ChatMessage[] messages = [new SystemChatMessage(systemPrompt), new UserChatMessage(message)];
            ChatCompletion completion = await _chatClient.CompleteChatAsync(messages);
            return completion.Content[0].Text;
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

            ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options);

            JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);

            return structuredJson;

        }
    }
}
