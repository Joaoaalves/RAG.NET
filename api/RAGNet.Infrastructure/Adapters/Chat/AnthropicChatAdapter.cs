using System.Text.Json;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Exceptions.Adapters;

namespace RAGNET.Infrastructure.Adapters.Chat
{
    public class AnthropicChatAdapter(string apiKey, string model) : IChatCompletionService
    {
        private readonly AnthropicClient _chatClient = new(apiKey);
        private readonly string _model = model ?? AnthropicModels.Claude3Haiku;

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {

            var messages = BuildMessages([systemPrompt, message]);
            var parameters = BuildParameters(messages);

            try
            {
                var result = await _chatClient.Messages.GetClaudeMessageAsync(parameters);

                if (result.FirstMessage.Text != null)
                {
                    return result.FirstMessage.Text;
                }

                throw new AnthropicChatException("Anthropic API returned an empty response.");
            }
            catch (Exception ex)
            {
                throw new AnthropicChatException("Error occurred while calling Anthropic API.", ex);
            }
        }

        public Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName)
        {
            throw new NotImplementedException();
        }

        private static List<Message> BuildMessages(string[] messages)
        {
            return
            [
                new(RoleType.User, messages[0]),
                new(RoleType.User, messages[1])
            ];
        }

        private MessageParameters BuildParameters(List<Message> messages, int? maxToken = 4096, bool? stream = false)
        {
            return new MessageParameters()
            {
                Messages = messages,
                MaxTokens = maxToken ?? 4096,
                Model = _model,
                Stream = stream ?? false,
            };
        }

        public static List<ConversationModel> GetModels()
        {
            return
            [
                new ConversationModel
                {
                    Label = "Claude 3.7 Sonnet",
                    Value = AnthropicModels.Claude37Sonnet,
                    Speed = 4,
                    InputPrice = 3,
                    OutputPrice = 15,
                    MaxOutput = 8192,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3.5 Sonnet",
                    Value = AnthropicModels.Claude35Sonnet,
                    Speed = 5,
                    InputPrice = 3,
                    OutputPrice = 15,
                    MaxOutput = 8192,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3.5 Haiku",
                    Value = AnthropicModels.Claude35Haiku,
                    Speed = 7,
                    InputPrice = 0.8f,
                    OutputPrice = 15,
                    MaxOutput = 8192,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3 Opus",
                    Value = AnthropicModels.Claude3Opus,
                    Speed = 6,
                    InputPrice = 15,
                    OutputPrice = 75,
                    MaxOutput = 4096,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3 Haiku",
                    Value = AnthropicModels.Claude3Haiku,
                    Speed = 8,
                    InputPrice = 0.25f,
                    OutputPrice = 1.25f,
                    MaxOutput = 4096,
                    ContextWindow = 200000
                },
            ];
        }
    }
}