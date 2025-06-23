
using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.CallbackUrls.CreateCallbackUrl
{
    public class CreateCallbackUrlCommand(
        WorkflowId workflowId,
        string url
    ) : UserAwareCommand<CallbackUrlDTO>
    {
        public WorkflowId WorkflowId { get; } = workflowId;
        public string Url { get; } = url;
    }
}