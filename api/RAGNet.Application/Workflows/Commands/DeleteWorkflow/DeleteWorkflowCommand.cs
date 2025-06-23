using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.Commands.DeleteWorkflow
{
    public class DeleteWorkflowCommand(WorkflowId workflowId) : UserAwareCommand<bool>
    {
        public WorkflowId WorkflowId { get; } = workflowId;
    }
}