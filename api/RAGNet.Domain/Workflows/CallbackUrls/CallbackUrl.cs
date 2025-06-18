using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.URLs;

namespace RAGNET.Domain.Workflows.CallbackUrls
{
    public class CallbackUrl : Entity
    {
        public CallbackUrlId Id { get; private init; } = default!;
        public URL Url { get; private set; } = default!;
        public WorkflowId WorkflowId { get; private set; } = null!;

        // EF Core
        private CallbackUrl() { }

        private CallbackUrl(CallbackUrlId id, URL url, WorkflowId workflowId)
        {
            Id = id;
            Url = url;
            WorkflowId = workflowId;
        }

        public static CallbackUrl Create(URL url, WorkflowId workflowId, CallbackUrlId? id = null)
        {
            if (workflowId == new WorkflowId(Guid.Empty)) throw new ArgumentException("Workflow ID cannot be empty.", nameof(workflowId));

            return new CallbackUrl(id ?? new CallbackUrlId(), url, workflowId);
        }

        public void SetUrl(URL url)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public override string ToString()
        {
            return Url.ToString();
        }

        public static implicit operator URL(CallbackUrl callbackUrl)
        {
            return callbackUrl.Url;
        }
    }
}