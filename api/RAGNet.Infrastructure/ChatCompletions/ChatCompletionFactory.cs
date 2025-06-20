using RAGNET.Application.Providers.Conversation;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.ChatCompletions
{
    public class ChatCompletionFactory : IChatCompletionFactory
    {
        public IChatCompletionService CreateCompletionService(string userApiKey, ConversationProviderConfig config)
        {

            return config.Provider switch
            {
                ConversationProviderEnum.OPENAI => new OpenAIChatAdapter(userApiKey, config.Model),
                ConversationProviderEnum.ANTHROPIC => new AnthropicChatAdapter(userApiKey, config.Model),
                ConversationProviderEnum.GEMINI => new GeminiChatAdapter(userApiKey, config.Model),
                _ => throw new NotSupportedException("Conversation Provider not supported")
            };
        }
    }
}