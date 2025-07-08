using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs
{
    public class Job<TContext> where TContext : JobProcessingContext
    {
        public Guid JobId { get; set; } = Guid.NewGuid();
        public string ApiKey { get; set; } = String.Empty;
        public string UserId { get; set; } = null!;
        public List<string> CallbackUrls { get; set; } = [];
        public TContext Context { get; set; } = null!;
    }
}