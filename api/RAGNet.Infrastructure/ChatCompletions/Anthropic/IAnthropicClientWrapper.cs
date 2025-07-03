using Anthropic.SDK.Messaging;

namespace RAGNET.Infrastructure.ChatCompletions.Anthropic
{
    public interface IAnthropicClientWrapper
    {
        Task<MessageResponse> GetClaudeMessageAsync(MessageParameters parameters);
    }
}
