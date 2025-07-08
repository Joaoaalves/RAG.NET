using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers.Conversation.Mappers
{
    public static class ConversationProviderMapper
    {
        public static ConversationProviderConfig ToConversationProviderConfig(this ConversationProviderConfigDTO dto)
        {
            return new ConversationProviderConfig(
                provider: dto.ProviderId,
                model: dto.Model
            );
        }

        public static ConversationProviderConfigDTO ToDTOFromConversationProviderConfig(this ConversationProviderConfig conversationProvider)
        {
            return new ConversationProviderConfigDTO
            {
                ProviderId = conversationProvider.Provider,
                ProviderName = conversationProvider.Provider,
                Model = conversationProvider.Model
            };
        }

        public static SupportedProvider ToSupportedProvider(this ConversationProviderEnum provider)
        {
            return provider switch
            {
                ConversationProviderEnum.OPENAI => SupportedProvider.OPENAI,
                ConversationProviderEnum.ANTHROPIC => SupportedProvider.ANTHROPIC,
                ConversationProviderEnum.GEMINI => SupportedProvider.GEMINI,
                ConversationProviderEnum.DeepSeek => SupportedProvider.DEEPSEEK,
                ConversationProviderEnum.XAI => SupportedProvider.XAI,
                ConversationProviderEnum.Mistral => SupportedProvider.MISTRAL,
                _ => throw new ArgumentOutOfRangeException("Unsupported Conversation Provider")
            };
        }
    }
}