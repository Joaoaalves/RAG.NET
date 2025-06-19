using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Application.Workflows.CallbackUrls.DeleteCallbackUrl
{
    public class DeleteCallbackUrlCommand(
        WorkflowId workflowId,
        string userId,
        CallbackUrlId callbackUrlId
    ) : ICommand<bool>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public WorkflowId WorkflowId { get; set; } = workflowId;
        public string UserId { get; set; } = userId;
        public CallbackUrlId CallbackUrlId { get; set; } = callbackUrlId;
    }
}