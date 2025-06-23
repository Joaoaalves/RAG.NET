using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Workflows.Queries.GetWorkflowDetails;

namespace RAGNET.Application.Workflows.Queries.GetUserWorkflows
{
    public class GetUserWorkflowsQuery(
    ) : UserAwareQuery<List<WorkflowDetailsDTO>>
    {
    }
}