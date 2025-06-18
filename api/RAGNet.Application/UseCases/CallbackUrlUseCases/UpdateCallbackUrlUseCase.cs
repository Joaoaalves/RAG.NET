using RAGNET.Application.DTOs.CallbackUrl;

using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.UseCases.CallbackUrlUseCases
{
    public interface IUpdateCallbackUrlUseCase
    {
        Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, Guid callbackUrlId, WorkflowId workflowId, string userId);
    }
    public class UpdateCallbackUrlUseCase(
        ICallbackUrlRepository callbackUrlRepository,
        IUnitOfWork unitOfWork
    ) : IUpdateCallbackUrlUseCase
    {
        private readonly ICallbackUrlRepository _callbackUrlRepository = callbackUrlRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CallbackUrlDTO> Execute(CallbackUrlDTO dto, Guid callbackUrlId, WorkflowId workflowId, string userId)
        {
            var callbackUrl = await _callbackUrlRepository.GetByIdAsync(callbackUrlId, workflowId)
                ?? throw new Exception("Invalid callback URL id");

            var url = URL.Create(dto.Url);
            callbackUrl.SetUrl(url);

            await _callbackUrlRepository.UpdateAsync(callbackUrl, workflowId);
            await _unitOfWork.CommitAsync();

            return dto;
        }

    }
}