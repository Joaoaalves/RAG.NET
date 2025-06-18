using RAGNET.Application.DTOs.CallbackUrl;
using RAGNET.Application.Mappers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.UseCases.CallbackUrlUseCases
{
    public interface IAddCallbackUrlUseCase
    {
        Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, WorkflowId workflowId, string userId);
    }
    public class AddCallbackUrlUseCase(
        IWorkflowRepository workflowRepository,
        IUnitOfWork unitOfWork
    ) : IAddCallbackUrlUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, WorkflowId workflowId, string userId)
        {
            var workflow = await _workflowRepository.GetByIdAsync(workflowId, userId) ?? throw new Exception("Invalid workflow id");

            var callback = dto.ToCallbackUrl(workflowId);

            workflow.AddCallbackUrl(callback);

            await _workflowRepository.UpdateAsync(workflow, userId);
            await _unitOfWork.CommitAsync();

            return callback.ToDTO();
        }

    }
}