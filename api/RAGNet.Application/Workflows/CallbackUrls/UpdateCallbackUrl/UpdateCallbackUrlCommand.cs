using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.UpdateCallbackUrl
{
    public class UpdateCallbackUrlCommand(
        WorkflowId workflowId,
        string userId,
        CallbackUrlId callbackUrlId,
        URL url
    ) : ICommand<CallbackUrlDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();

        public WorkflowId WorkflowId { get; } = workflowId;
        public string UserId { get; } = userId;
        public CallbackUrlId CallbackUrlId { get; } = callbackUrlId;
        public URL Url { get; } = url;
    }
}