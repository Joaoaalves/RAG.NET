using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.UseCases.CallbackUrlUseCases
{
    public interface IDeleteCallbackUrlUseCase
    {
        Task<CallbackUrlId> Execute(CallbackUrlId callbackUrlId, WorkflowId workflowId, string userId);
    }
    public class DeleteCallbackUrlUseCase(
        ICallbackUrlRepository callbackUrlRepository,
        IUnitOfWork unitOfWork
    ) : IDeleteCallbackUrlUseCase
    {
        private readonly ICallbackUrlRepository _callbackUrlRepository = callbackUrlRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<CallbackUrlId> Execute(CallbackUrlId callbackUrlId, WorkflowId workflowId, string userId)
        {
            var callbackUrl = await _callbackUrlRepository.GetByIdAsync(callbackUrlId, workflowId) ?? throw new Exception("Invalid workflowid");

            await _callbackUrlRepository.DeleteAsync(callbackUrl, workflowId);

            await _unitOfWork.CommitAsync();

            return callbackUrlId;
        }

    }
}