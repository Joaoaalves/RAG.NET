using RAGNET.Domain.Workflows;

using RAGNET.Application.Workflows.Queries.GetWorkflowDetails;
using RAGNET.Application.Infrastructure.Providers.Embedding.Mappers;
using RAGNET.Application.Infrastructure.Providers.Conversation.Mappers;
using RAGNET.Application.QueryEnhancers.Mappers;
using RAGNET.Application.Chunkers.Mappers;
using RAGNET.Application.QueryResultFilters.Mappers;
using RAGNET.Application.Workflows.CallbackUrls.Mappers;

namespace RAGNET.Application.Workflows.Mappers
{
    public static class WorkflowMapper
    {

        public static WorkflowDetailsDTO ToWorkflowDetailsDTO(
            this Workflow workflow)
        {
            var chunkerSettings = workflow.Chunker!.Metas.ToDictionary(m => m.Key, m => m.Value).ToChunkerSettingsDTO();

            return new WorkflowDetailsDTO
            {
                Id = workflow.Id.Value,
                Name = workflow.Name,
                Description = workflow.Description,
                IsActive = workflow.IsActive,
                DocumentsCount = workflow.DocumentsCount,
                CollectionId = workflow.CollectionId,
                Strategy = workflow.Chunker!.StrategyType,
                ApiKey = workflow.ApiKey.Value,
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