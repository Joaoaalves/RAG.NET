using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.URLs;

namespace RAGNET.Domain.Workflows.CallbackUrls
{
    public class CallbackUrl : Entity
    {
        public Guid Id { get; private init; } = default!;
        public URL Url { get; private set; } = default!;
        public Guid WorkflowId { get; private set; }

        // EF Core
        private CallbackUrl() { }

        private CallbackUrl(Guid id, URL url, Guid workflowId)
        {
            Id = id;
            Url = url;
            WorkflowId = workflowId;
        }

        public static CallbackUrl Create(URL url, Guid workflowId, Guid? id = null)
        {
            if (workflowId == Guid.Empty) throw new ArgumentException("Workflow ID cannot be empty.", nameof(workflowId));

            return new CallbackUrl(id ?? Guid.NewGuid(), url, workflowId);
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