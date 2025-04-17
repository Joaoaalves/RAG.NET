using RAGNET.Application.DTOs.Embedder;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Mappers
{
    public static class ConversationProviderMapper
    {
        public static ConversationProviderConfig ToConversationProviderConfig(this ConversationProviderConfigDTO dto, Guid workflowId)
        {
            return new ConversationProviderConfig
            {
                Provider = dto.Provider,
                Model = dto.Model,
                WorkflowId = workflowId
            };
        }

        public static ConversationProviderConfigDTO ToDTOFromConversationProviderConfig(this ConversationProviderConfig conversationProvider)
        {
            return new ConversationProviderConfigDTO
            {
                Provider = conversationProvider.Provider,
                Model = conversationProvider.Model
            };
        }

        public static SupportedProvider ToSupportedProvider(this ConversationProviderEnum provider)
        {
            return provider switch
            {
                ConversationProviderEnum.OPENAI => SupportedProvider.OpenAI,
                ConversationProviderEnum.ANTHROPIC => SupportedProvider.Anthropic,
                _ => throw new ArgumentOutOfRangeException("Unsupported provider")
            };
        }
    }
}