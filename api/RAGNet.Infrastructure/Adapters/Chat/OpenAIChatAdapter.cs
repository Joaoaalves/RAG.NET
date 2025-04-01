using System.Text.Json;
using OpenAI.Chat;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Infrastructure.Adapters.Chat
{
    public class OpenAIChatAdapter : IChatCompletionService
    {
        private readonly ChatClient _chatClient;
        public OpenAIChatAdapter(string apiKey, string model)
        {
            _chatClient = new(model, apiKey);
        }

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


        public static List<ConversationModel> GetModels()
        {
            return [
                new ConversationModel
                {
                    Label = "GPT-3.5 Turbo",
                    Value = "gpt-3.5-turbo",
                    Speed = 3,
                    InputPrice = 0.5f,
                    OutputPrice = 1.5f,
                    MaxOutput = 4096,
                    ContextWindow = 16384
                },
                new ConversationModel
                {
                    Label = "GPT-4o mini",
                    Value = "gpt-4o-mini-2024-07-18",
                    Speed = 6,
                    InputPrice = 0.15f,
                    OutputPrice = 0.6f,
                    MaxOutput = 16384,
                    ContextWindow = 128000
                },
                new ConversationModel
                {
                    Label = "GPT-4",
                    Value = "gpt-4",
                    Speed = 3,
                    InputPrice = 30.0f,
                    OutputPrice = 60.0f,
                    MaxOutput = 8192,
                    ContextWindow = 8192
                },
                new ConversationModel
                {
                    Label = "GPT-4 Turbo",
                    Value = "gpt-4-turbo",
                    Speed = 5,
                    InputPrice = 10.0f,
                    OutputPrice = 30.0f,
                    MaxOutput = 4096,
                    ContextWindow = 128000
                },
                new ConversationModel
                {
                    Label = "GPT-4o",
                    Value = "gpt-4o-2024-08-06",
                    Speed = 5,
                    InputPrice = 5f,
                    OutputPrice = 15f,
                    MaxOutput = 16384,
                    ContextWindow = 128000
                },
                new ConversationModel
                {
                    Label = "GPT-4.5 Preview",
                    Value = "gpt-4.5-preview-2025-02-27",
                    Speed = 5,
                    InputPrice = 75,
                    OutputPrice = 150,
                    MaxOutput = 16384,
                    ContextWindow = 128000
                },
                new ConversationModel
                {
                    Label = "o1-mini",
                    Value = "o1-mini-2024-09-12",
                    Speed = 3,
                    InputPrice = 1.1f,
                    OutputPrice = 4.4f,
                    MaxOutput = 65536,
                    ContextWindow = 128000
                },
                new ConversationModel
                {
                    Label = "o3-mini",
                    Value = "o3-mini-2025-01-31",
                    Speed = 4,
                    InputPrice = 1.1f,
                    OutputPrice = 4.4f,
                    MaxOutput = 100000,
                    ContextWindow = 200000
                },
            ];
        }
    }
}
