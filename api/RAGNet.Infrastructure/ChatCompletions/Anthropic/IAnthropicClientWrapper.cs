using Anthropic.SDK.Messaging;

namespace RAGNET.Infrastructure.ChatCompletions.Anthropic
{
    public interface IAnthropicClientWrapper
    {
        Task<MessageResponse> CompleteChatAsync(MessageParameters parameters);
    }
}
