using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Workflows.GetWorkflowDetails;

namespace RAGNET.Application.Workflows.GetUserWorkflows
{
    public class GetUserWorkflowsQuery(
    ) : UserAwareQuery<List<WorkflowDetailsDTO>>
    {
    }
}