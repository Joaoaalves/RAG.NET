using System.Text.Json;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Infrastructure.Exceptions.Adapters;
using RAGNET.Infrastructure.SeedWork.Resilience;

namespace RAGNET.Infrastructure.ChatCompletions.Anthropic
{
    public class AnthropicChatAdapter(IAnthropicClientWrapper client, string model = AnthropicModels.Claude3Haiku, int delayMs = 2000) : IConversationProviderService
    {
        private readonly IAnthropicClientWrapper _client = client;
        private readonly string _model = model;
        private readonly int _delayMs = delayMs;

        public async Task<string> GetCompletionAsync(string systemPrompt, string message)
        {
            var parameters = BuildParameters([systemPrompt, message]);

            try
            {
                var result = await RetryHelper.ExecuteWithRetryAsync(() =>
                    _client.GetClaudeMessageAsync(parameters), baseDelayMs: _delayMs);

                return result.FirstMessage.Text
                    ?? throw new AnthropicChatException("Anthropic API returned an empty response.");
            }
            catch (JsonException je)
            {
                throw new AnthropicChatException("Failed to parse response from Anthropic.", je);
            }
            catch (Exception ex)
            {
                throw new AnthropicChatException("Error occurred while calling Anthropic API.", ex);
            }
        }

        public async Task<JsonDocument> GetCompletionStructuredAsync(string systemPrompt, string message, JsonDocument jsonSchema, string? formatName)
        {
            var structuredMessage = $"{systemPrompt}\nThe response should follow this JSON format: {jsonSchema.RootElement}";
            var parameters = BuildParameters([structuredMessage, message]);

            try
            {
                var result = await RetryHelper.ExecuteWithRetryAsync(() =>
                    _client.GetClaudeMessageAsync(parameters), baseDelayMs: _delayMs);

                var jsonText = result.FirstMessage.Text ?? "{}";
                return JsonDocument.Parse(jsonText);
            }
            catch (JsonException je)
            {
                throw new AnthropicChatException("Failed to parse structured JSON from Anthropic.", je);
            }
            catch (Exception ex)
            {
                throw new AnthropicChatException("Error occurred while calling Anthropic API for structured response.", ex);
            }
        }

        private MessageParameters BuildParameters(string[] messages)
        {
            return new MessageParameters
            {
                Messages =
                [
                    new(RoleType.User, messages[0]),
                    new(RoleType.User, messages[1])
                ],
                MaxTokens = 4096,
                Model = _model,
                Stream = false
            };
        }
    }
}
