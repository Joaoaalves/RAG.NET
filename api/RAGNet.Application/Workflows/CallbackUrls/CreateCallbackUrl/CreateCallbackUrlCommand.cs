
using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.CallbackUrls.CreateCallbackUrl
{
    public class CreateCallbackUrlCommand(
        WorkflowId workflowId,
        string userId,
        string url
    ) : ICommand<CallbackUrlDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public WorkflowId WorkflowId { get; } = workflowId;
        public string UserId { get; } = userId;
        public string Url { get; } = url;
    }
}