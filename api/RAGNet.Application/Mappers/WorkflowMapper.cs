using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.Conversation;
using RAGNET.Application.DTOs.Embedding;
using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Application.DTOs.Workflow;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Mappers
{
    public static class WorkflowMapper
    {

        public static WorkflowDetailsDTO ToWorkflowDetailsDTOFromWorkflow(
            this Workflow workflow,
            ChunkerStrategy strategy,
            ChunkerSettingsDTO settings,
            EmbeddingProviderConfigDTO embedding,
            ConversationProviderConfigDTO conversationProvider,
            List<QueryEnhancerDTO>? queryEnhancers,
            List<CallbackUrlDTO>? callbackUrls)
        {
            return new WorkflowDetailsDTO
            {
                Id = workflow.Id.Value,
                Name = workflow.Name,
                Description = workflow.Description,
                IsActive = workflow.IsActive,
                DocumentsCount = workflow.DocumentsCount,
                CollectionId = workflow.CollectionId,
                Strategy = strategy,
                ApiKey = workflow.ApiKey,
                Settings = settings,
                EmbeddingProvider = embedding,
                ConversationProvider = conversationProvider,
                Filter = workflow.Filter?.ToDTO(),
                CallbackUrls = callbackUrls ?? [],
                QueryEnhancers = queryEnhancers ?? []
            };
        }
    }
}