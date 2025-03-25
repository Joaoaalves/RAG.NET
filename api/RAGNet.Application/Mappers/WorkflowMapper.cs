using RAGNET.Application.DTOs.Chunker;
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
                ApiKey = Guid.NewGuid().ToString("N"),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static WorkflowDetailsDTO ToWorkflowDetailsDTOFromWorkflow(this Workflow workflow, ChunkerStrategy strategy, ChunkerSettingsDTO settings)
        {
            return new WorkflowDetailsDTO
            {
                Name = workflow.Name,
                Strategy = strategy,
                ApiKey = workflow.ApiKey,
                Settings = settings,
            };
        }
    }
}