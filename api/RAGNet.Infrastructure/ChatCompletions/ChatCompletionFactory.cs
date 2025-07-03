using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Infrastructure.ChatCompletions.Anthropic;
using RAGNET.Infrastructure.ChatCompletions.DeepSeek;
using RAGNET.Infrastructure.ChatCompletions.Gemini;
using RAGNET.Infrastructure.ChatCompletions.Mistral;
using RAGNET.Infrastructure.ChatCompletions.OpenAI;
using RAGNET.Infrastructure.ChatCompletions.xAI;

namespace RAGNET.Infrastructure.ChatCompletions
{
    public class ChatCompletionFactory : IConversationProviderFactory
    {
        private readonly int _baseDelayMS = 1;
        public IConversationProviderService CreateCompletionService(string apiKey, ConversationProviderConfig config)
        {
            var model = config.Model;

            return config.Provider switch
            {
                ConversationProviderEnum.OPENAI => OpenAIClient(apiKey, model),
                ConversationProviderEnum.ANTHROPIC => AnthropicClient(apiKey, model),
                ConversationProviderEnum.GEMINI => GeminiClient(apiKey, model),
                ConversationProviderEnum.DeepSeek => DeepSeekClient(apiKey, model),
                ConversationProviderEnum.XAI => XAIClient(apiKey, model),
                ConversationProviderEnum.Mistral => MistralClient(apiKey, model),
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

        private XAIChatAdapter XAIClient(string apiKey, string model)
        {
            var clientWrapper = new XAIChatClientWrapper(apiKey, model);

            return new XAIChatAdapter(clientWrapper, _baseDelayMS);
        }

        private MistralChatAdapter MistralClient(string apiKey, string model)
        {
            return new MistralChatAdapter(apiKey, model, delayMs: _baseDelayMS);
        }
    }
}