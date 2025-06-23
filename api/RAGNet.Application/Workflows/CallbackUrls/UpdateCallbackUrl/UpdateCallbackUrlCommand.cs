using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.UpdateCallbackUrl
{
    public class UpdateCallbackUrlCommand(
        WorkflowId workflowId,
        CallbackUrlId callbackUrlId,
        URL url
    ) : UserAwareCommand<CallbackUrlDTO>
    {
        public WorkflowId WorkflowId { get; } = workflowId;
        public CallbackUrlId CallbackUrlId { get; } = callbackUrlId;
        public URL Url { get; } = url;
    }
}