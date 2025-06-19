using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.DeleteWorkflow
{
    public class DeleteWorkflowCommand(WorkflowId workflowId, string userId) : ICommand<bool>
    {
        public Guid Id { get; } = Guid.NewGuid();

        public WorkflowId WorkflowId { get; } = workflowId;
        public string UserId { get; } = userId;
    }
}