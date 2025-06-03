using System.Text.Json;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using RAGNET.Domain.Services;
using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Infrastructure.Exceptions.Adapters;

namespace RAGNET.Infrastructure.Adapters.Chat.Anthropic
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
                return result.FirstMessage.Text ?? throw new AnthropicChatException("Anthropic API returned an empty response.");
            }
            catch (Exception ex)
            {
                throw new AnthropicChatException("Error occurred while calling Anthropic API.", ex);
            }
        }

        public async Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName)
        {
            var structuredMessage = $"{systemPrompt}\nThe response should follow this JSON format: {jsonSchema.RootElement}";
            var messages = BuildMessages([structuredMessage, message]);
            var parameters = BuildParameters(messages);

            try
            {
                var result = await _chatClient.Messages.GetClaudeMessageAsync(parameters);
                return JsonDocument.Parse(result.FirstMessage.Text ?? "{}");
            }
            catch (Exception ex)
            {
                throw new AnthropicChatException("Error occurred while calling Anthropic API for structured response.", ex);
            }
        }

        private static List<Message> BuildMessages(string[] messages)
        {
            return [
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
    }
}