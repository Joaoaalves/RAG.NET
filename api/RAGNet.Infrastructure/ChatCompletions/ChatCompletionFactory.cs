using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Infrastructure.ChatCompletions.Anthropic;
using RAGNET.Infrastructure.ChatCompletions.DeepSeek;
using RAGNET.Infrastructure.ChatCompletions.Gemini;
using RAGNET.Infrastructure.ChatCompletions.OpenAI;

namespace RAGNET.Infrastructure.ChatCompletions
{
    public class ChatCompletionFactory : IConversationProviderFactory
    {
        private readonly int _baseDelayMS = 1;
        public IConversationProviderService CreateCompletionService(string userApiKey, ConversationProviderConfig config)
        {

            return config.Provider switch
            {
                ConversationProviderEnum.OPENAI => OpenAIClient(userApiKey, config.Model),
                ConversationProviderEnum.ANTHROPIC => AnthropicClient(userApiKey, config.Model),
                ConversationProviderEnum.GEMINI => GeminiClient(userApiKey, config.Model),
                ConversationProviderEnum.DeepSeek => DeepSeekClient(userApiKey, config.Model),
                _ => throw new NotSupportedException("Conversation Provider not supported")
            };
        }

        private OpenAIChatAdapter OpenAIClient(string apiKey, string model)
        {
            return new OpenAIChatAdapter(new OpenAIChatClientWrapper(apiKey, model), delayMs: _baseDelayMS);
        }

        private AnthropicChatAdapter AnthropicClient(string apiKey, string model)
        {
            return new AnthropicChatAdapter(new AnthropicClientWrapper(apiKey), model, delayMs: _baseDelayMS);
        }

        private GeminiChatAdapter GeminiClient(string apiKey, string model)
        {
            return new GeminiChatAdapter(apiKey, model, delayMs: _baseDelayMS);
        }

        private DeepSeekChatAdapter DeepSeekClient(string apiKey, string model)
        {
            var clientWrapper = new DeepSeekClientWrapper(apiKey, model);

            return new DeepSeekChatAdapter(clientWrapper, delayMs: _baseDelayMS);
        }
    }
}