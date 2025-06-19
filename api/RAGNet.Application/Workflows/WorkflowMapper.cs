using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Workflows;

using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.Conversation;
using RAGNET.Application.DTOs.Embedding;
using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.Workflows.GetWorkflowDetails;
using RAGNET.Application.QueryResultFilters;
using RAGNET.Application.Mappers;

namespace RAGNET.Application.Workflows
{
    public static class WorkflowMapper
    {

        public static WorkflowDetailsDTO ToWorkflowDetailsDTOFromWorkflow(
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
                Filter = workflow.Filter?.ToDTO(),
                CallbackUrls = workflow.CallbackUrls.ToDTOList() ?? [],
                QueryEnhancers = workflow.QueryEnhancers.ToDTOList() ?? []
            };
        }
    }
}