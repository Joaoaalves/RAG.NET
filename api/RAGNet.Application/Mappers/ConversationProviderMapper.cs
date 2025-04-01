using RAGNET.Application.DTOs.Embedder;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.Mappers
{
    public static class ConversationProviderMapper
    {
        public static ConversationProviderConfig ToConversationProviderConfigFromConversationProviderConfigDTO(this ConversationProviderConfigDTO dto, Guid workflowId)
        {
            return new ConversationProviderConfig
            {
                Provider = dto.Provider,
                ApiKey = dto.ApiKey,
                Model = dto.Model,
                WorkflowId = workflowId
            };
        }

        public static ConversationProviderConfigDTO ToDTOFromConversationProviderConfig(this ConversationProviderConfig conversationProvider)
        {
            return new ConversationProviderConfigDTO
            {
                Provider = conversationProvider.Provider,
                ApiKey = conversationProvider.ApiKey,
                Model = conversationProvider.Model
            };
        }
    }
}