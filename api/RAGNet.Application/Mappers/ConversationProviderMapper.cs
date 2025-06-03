using RAGNET.Application.DTOs.Conversation;
using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Mappers
{
    public static class ConversationProviderMapper
    {
        public static ConversationProviderConfig ToConversationProviderConfig(this ConversationProviderConfigDTO dto, Guid workflowId)
        {
            return new ConversationProviderConfig
            {
                Provider = dto.ProviderId,
                Model = dto.Model,
                WorkflowId = workflowId
            };
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