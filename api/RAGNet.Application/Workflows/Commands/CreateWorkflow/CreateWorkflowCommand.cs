using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{
    public class CreateWorkflowCommand(WorkflowCreationDTO dto) : UserAwareCommand<WorkflowId>
    {
        public WorkflowCreationDTO Dto { get; } = dto;
    }
}