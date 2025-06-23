using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.Commands.DeleteCallbackUrl
{
    public class DeleteCallbackUrlCommandHandler(
        ICallbackUrlRepository callbackUrlRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteCallbackUrlCommand, bool>
    {
        private readonly ICallbackUrlRepository _callbackUrlRepository = callbackUrlRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> Handle(DeleteCallbackUrlCommand request, CancellationToken cancellationToken)
        {
            var callbackUrl = await _callbackUrlRepository.GetByIdAsync(request.CallbackUrlId, request.WorkflowId) ?? throw new Exception("Invalid workflowid");

            await _callbackUrlRepository.DeleteAsync(callbackUrl, request.WorkflowId);

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}