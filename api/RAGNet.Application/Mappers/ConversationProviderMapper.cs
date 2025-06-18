using RAGNET.Application.DTOs.Conversation;

using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Mappers
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
                ConversationProviderEnum.OPENAI => SupportedProvider.OpenAI,
                ConversationProviderEnum.ANTHROPIC => SupportedProvider.Anthropic,
                ConversationProviderEnum.GEMINI => SupportedProvider.Gemini,
                _ => throw new ArgumentOutOfRangeException("Unsupported provider")
            };
        }
    }
}