using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Infrastructure.Jobs
{
    public interface IJobProcessingHandler
    {
        void SetNext(IJobProcessingHandler next);
        Task HandleAsync(EmbeddingJob job, CancellationToken ct);
    }
    public abstract class BaseJobProcessingHandler : IJobProcessingHandler
    {
        private IJobProcessingHandler? _next;

        public void SetNext(IJobProcessingHandler next)
        {
            _next = next;
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