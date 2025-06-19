using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Workflows.GetWorkflowDetails;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.GetUserWorkflows
{
    public class GetUserWorkflowsQueryHandler(
        IWorkflowRepository workflowRepository
    ) : IQueryHandler<GetUserWorkflowsQuery, List<WorkflowDetailsDTO>>
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        public async Task<List<WorkflowDetailsDTO>> Handle(GetUserWorkflowsQuery request, CancellationToken cancellationToken)
        {
            var workflows = await _workflowRepository.GetUserWorkflows(request.UserId);
            var workflowsDTO = new List<WorkflowDetailsDTO>();

            foreach (var workflow in workflows)
                workflowsDTO.Add(workflow.ToWorkflowDetailsDTOFromWorkflow());

            return workflowsDTO;
        }
    }
}