using RAGNET.Domain.Entities.Jobs;

namespace RAGNet.Domain.Services
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