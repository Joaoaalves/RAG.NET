using OpenAI.Chat;

namespace RAGNET.Infrastructure.ChatCompletions.OpenAI
{
    public class OpenAIChatClientWrapper(string apiKey, string model) : IOpenAIChatClientWrapper
    {
        private readonly ChatClient _client = new(model, apiKey);

        public async Task<string> CompleteChatAsync(ChatMessage[] messages, ChatCompletionOptions? options = null, CancellationToken cancellationToken = default)
        {
            var completion = await _client.CompleteChatAsync(messages, options, cancellationToken);
            return completion.Value.Content[0].Text;
        }
    }
}
