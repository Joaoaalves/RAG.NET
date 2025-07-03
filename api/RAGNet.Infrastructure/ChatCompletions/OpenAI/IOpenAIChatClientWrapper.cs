using OpenAI.Chat;

namespace RAGNET.Infrastructure.ChatCompletions.OpenAI
{
    public interface IOpenAIChatClientWrapper
    {
        Task<string> CompleteChatAsync(ChatMessage[] messages, ChatCompletionOptions? options = null, CancellationToken cancellationToken = default);
    }
}
