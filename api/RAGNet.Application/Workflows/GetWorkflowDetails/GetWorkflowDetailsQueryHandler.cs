using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.GetWorkflowDetails
{
    public class GetWorkflowDetailsQueryHandler(
        IWorkflowRepository workflowRepository
    ) : IQueryHandler<GetWorkflowDetailsQuery, WorkflowDetailsDTO>
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        public async Task<WorkflowDetailsDTO> Handle(GetWorkflowDetailsQuery request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId, request.User.Id) ?? throw new Exception("Workflow não encontrado.");

            return workflow.ToWorkflowDetailsDTO();
        }
    }
}