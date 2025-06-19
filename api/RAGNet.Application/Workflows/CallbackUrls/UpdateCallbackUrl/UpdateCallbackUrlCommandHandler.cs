using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.UpdateCallbackUrl
{
    public class UpdateCallbackUrlCommandHandler(
        ICallbackUrlRepository callbackUrlRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateCallbackUrlCommand, CallbackUrlDTO>
    {
        private readonly ICallbackUrlRepository _callbackUrlRepository = callbackUrlRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<CallbackUrlDTO> Handle(UpdateCallbackUrlCommand request, CancellationToken cancellationToken)
        {
            var callbackUrl = await _callbackUrlRepository.GetByIdAsync(request.CallbackUrlId, request.WorkflowId)
                ?? throw new Exception("Invalid callback URL id");

            callbackUrl.SetUrl(request.Url);

            await _callbackUrlRepository.UpdateAsync(callbackUrl, request.WorkflowId);
            await _unitOfWork.CommitAsync(cancellationToken);

            return callbackUrl.ToDTO();
        }
    }
}