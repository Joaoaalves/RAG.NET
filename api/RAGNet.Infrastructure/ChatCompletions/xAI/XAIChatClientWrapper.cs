using System.ClientModel;
using OpenAI;
using OpenAI.Chat;
using RAGNET.Infrastructure.ChatCompletions.OpenAI;

namespace RAGNET.Infrastructure.ChatCompletions.xAI
{
    public class XAIChatClientWrapper : IOpenAIChatClientWrapper
    {
        private readonly ChatClient _client;

        public XAIChatClientWrapper(string apiKey, string model)
        {
            _client = new ChatClient(
                model,
                credential: new ApiKeyCredential(apiKey),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://api.x.ai/v1")
                }
            );
        }
        public async Task<string> CompleteChatAsync(ChatMessage[] messages, ChatCompletionOptions? options = null, CancellationToken cancellationToken = default)
        {
            var completion = await _client.CompleteChatAsync(messages, options, cancellationToken);
            return completion.Value.Content[0].Text;
        }
    }
}