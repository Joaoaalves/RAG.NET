using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.Embedder;
using RAGNET.Application.DTOs.Workflow;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.Mappers
{
    public static class WorkflowMapper
    {
        public static Workflow ToWorkflowFromCreationDTO(this WorkflowCreationDTO dto, User user)
        {
            return new Workflow
            {
                Name = dto.Name,
                Description = dto.Description,
                Documents = 0,
                ApiKey = Guid.NewGuid().ToString("N"),
                UserId = user.Id,
                CollectionId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static WorkflowDetailsDTO ToWorkflowDetailsDTOFromWorkflow(
            this Workflow workflow,
            ChunkerStrategy strategy, ChunkerSettingsDTO settings,
            EmbeddingProviderConfigDTO embedding, ConversationProviderConfigDTO conversationProvider)
        {
            return new WorkflowDetailsDTO
            {
                Id = workflow.Id,
                Name = workflow.Name,
                Description = workflow.Description,
                Documents = workflow.Documents,
                Strategy = strategy,
                ApiKey = workflow.ApiKey,
                Settings = settings,
                EmbeddingProvider = embedding,
                ConversationProvider = conversationProvider
            };
        }
    }
}