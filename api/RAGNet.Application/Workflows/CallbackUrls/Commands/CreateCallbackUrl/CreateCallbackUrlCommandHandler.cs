using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Workflows.CallbackUrls.DTOs;
using RAGNET.Application.Workflows.CallbackUrls.Mappers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.Commands.CreateCallbackUrl
{
    public class CreateCallbackUrlCommandHandler(
        IWorkflowRepository workflowRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateCallbackUrlCommand, CallbackUrlDTO>
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<CallbackUrlDTO> Handle(CreateCallbackUrlCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _workflowRepository.GetByIdAsync(request.WorkflowId, request.User.Id) ?? throw new Exception("Invalid workflow id");

            var url = URL.Create(request.Url);

            var callback = CallbackUrl.Create(
                url,
                workflow.Id
            );

            workflow.AddCallbackUrl(callback);

            await _workflowRepository.UpdateAsync(workflow, request.Url);
            await _unitOfWork.CommitAsync(cancellationToken);

            return callback.ToDTO();
        }
    }
}