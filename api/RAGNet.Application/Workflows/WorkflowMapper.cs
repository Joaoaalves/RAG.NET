using RAGNET.Domain.Workflows;

using RAGNET.Application.Workflows.GetWorkflowDetails;
using RAGNET.Application.QueryResultFilters;
using RAGNET.Application.Mappers;
using RAGNET.Application.Workflows.CallbackUrls;
using RAGNET.Application.QueryEnhancers;
using RAGNET.Application.Providers.Embedding;
using RAGNET.Application.Providers.Conversation;

namespace RAGNET.Application.Workflows
{
    public static class WorkflowMapper
    {

        public static WorkflowDetailsDTO ToWorkflowDetailsDTO(
            this Workflow workflow)
        {
            var chunkerSettings = workflow.Chunker!.Metas.ToDictionary(m => m.Key, m => m.Value).ToChunkerSettingsDTOfromDictionary();

            return new WorkflowDetailsDTO
            {
                Id = workflow.Id.Value,
                Name = workflow.Name,
                Description = workflow.Description,
                IsActive = workflow.IsActive,
                DocumentsCount = workflow.DocumentsCount,
                CollectionId = workflow.CollectionId,
                Strategy = workflow.Chunker!.StrategyType,
                ApiKey = workflow.ApiKey,
                Settings = chunkerSettings,
                EmbeddingProvider = workflow.EmbeddingProviderConfig.ToDTOFromEmbeddingProviderConfig(),
                ConversationProvider = workflow.ConversationProviderConfig.ToDTOFromConversationProviderConfig(),
                QueryResultFilter = workflow.QueryResultFilter?.ToDTO(),
                CallbackUrls = workflow.CallbackUrls.ToDTOList() ?? [],
                QueryEnhancers = workflow.QueryEnhancers.ToDTOList() ?? []
            };
        }
    }
}