using Anthropic.SDK;
using Anthropic.SDK.Messaging;

namespace RAGNET.Infrastructure.ChatCompletions.Anthropic
{
    public class AnthropicClientWrapper(string apiKey) : IAnthropicClientWrapper
    {
        private readonly AnthropicClient _client = new(apiKey);

        public Task<MessageResponse> CompleteChatAsync(MessageParameters parameters)
        {
            return _client.Messages.GetClaudeMessageAsync(parameters);
        }
    }
}
