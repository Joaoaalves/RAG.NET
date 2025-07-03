using DeepSeek.Core.Models;

namespace RAGNET.Infrastructure.ChatCompletions.DeepSeek
{
    public interface IDeepSeekClientWrapper
    {
        Task<string> CompleteChatAsync(List<Message> messages, ResponseFormat? responseFormat = null, CancellationToken ct = default);
    }
}