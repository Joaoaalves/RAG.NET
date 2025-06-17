using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IDeleteWorkflowUseCase
    {
        Task<bool> Execute(Guid workflowId, string userId);
    }
    public class DeleteWorkflowUseCase(IWorkflowRepository workflowRepository, IUnitOfWork unitOfWork) : IDeleteWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Execute(Guid workflowId, string userId)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId, userId) ?? throw new Exception("Workflow not found");
            await _workflowRepository.DeleteAsync(workflow, userId);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}