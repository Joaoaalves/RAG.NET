using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.Commands.DeleteWorkflow
{
    public class DeleteWorkflowCommandHandler(
        IWorkflowRepository workflowRepository, IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteWorkflowCommand, bool>
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteWorkflowCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId, request.User.Id) ?? throw new Exception("Workflow not found");
            await _workflowRepository.DeleteAsync(workflow, request.User.Id);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}