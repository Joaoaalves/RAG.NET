using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.GetWorkflowDetails
{
    public class GetWorkflowDetailsQuery(WorkflowId workflowId, string userId) : IQuery<WorkflowDetailsDTO>
    {
        public WorkflowId WorkflowId { get; } = workflowId;
        public string UserId { get; } = userId;
    }
}