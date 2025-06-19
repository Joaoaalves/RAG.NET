using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Workflows.CreateWorkflow
{
    public class CreateWorkflowCommand(WorkflowCreationDTO dto, User user) : ICommand<WorkflowId>
    {
        public Guid Id { get; } = Guid.NewGuid();

        public WorkflowCreationDTO Dto { get; } = dto;
        public User User { get; } = user;
    }
}