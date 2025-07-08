using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Infrastructure.Jobs
{
    public abstract class BaseJobProcessingHandler : IJobProcessingHandler
    {
        private IJobProcessingHandler? _next;

        public IJobProcessingHandler SetNext(IJobProcessingHandler next)
        {
            _next = next;
            return _next;
        }

        public virtual async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            if (_next != null)
            {
                await _next.HandleAsync(job, ct);
            }
        }
    }
}