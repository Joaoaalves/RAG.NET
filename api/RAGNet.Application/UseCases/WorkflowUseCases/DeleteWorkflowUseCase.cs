using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.WorkflowUseCases
{
    public interface IDeleteWorkflowUseCase
    {
        Task<bool> Execute(Guid workflowId, string userId);
    }
    public class DeleteWorkflowUseCase(IWorkflowRepository workflowRepository) : IDeleteWorkflowUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        public async Task<bool> Execute(Guid workflowId, string userId)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId, userId) ?? throw new Exception("Workflow not found");
            await _workflowRepository.DeleteAsync(workflow, userId);

            return true;
        }
    }
}