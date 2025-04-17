using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Chat;

namespace RAGNET.Infrastructure.Factories
{
    public class ChatCompletionFactory : IChatCompletionFactory
    {
        public IChatCompletionService CreateCompletionService(string userApiKey, ConversationProviderConfig config)
        {

            return config.Provider switch
            {
                ConversationProviderEnum.OPENAI => new OpenAIChatAdapter(userApiKey, config.Model),
                ConversationProviderEnum.ANTHROPIC => new AnthropicChatAdapter(userApiKey, config.Model),
                _ => throw new NotSupportedException("Conversation Provider not supported")
            };
        }
    }
}