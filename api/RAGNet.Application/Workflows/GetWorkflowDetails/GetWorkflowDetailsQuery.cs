using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.GetWorkflowDetails
{
    public class GetWorkflowDetailsQuery(
        WorkflowId workflowId
    ) : UserAwareQuery<WorkflowDetailsDTO>
    {
        public WorkflowId WorkflowId { get; } = workflowId;
    }
}