using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.DeleteCallbackUrl
{
    public class DeleteCallbackUrlCommand(
        WorkflowId workflowId,
        CallbackUrlId callbackUrlId
    ) : UserAwareCommand<bool>
    {
        public WorkflowId WorkflowId { get; set; } = workflowId;
        public CallbackUrlId CallbackUrlId { get; set; } = callbackUrlId;
    }
}