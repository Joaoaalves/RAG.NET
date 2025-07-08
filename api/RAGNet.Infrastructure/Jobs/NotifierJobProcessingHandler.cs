using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Jobs
{
    public abstract class NotifierJobProcessingHandler(
        IJobNotificationService realTimeNotifier
    ) : IJobProcessingHandler
    {
        private IJobProcessingHandler? _next;
        private readonly IJobNotificationService _realTimeNotifier = realTimeNotifier;
        public IJobProcessingHandler SetNext(IJobProcessingHandler next)
        {
            _next = next;
            return next;
        }

        public virtual async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            try
            {
                if (_next != null)
                {
                    await _next.HandleAsync(job, ct);
                }
            }
            catch (Exception exc)
            {
                await HandleErrorAsync(job, exc, ct);
                throw;
            }
        }

        protected virtual async Task HandleErrorAsync(EmbeddingJob job, Exception ex, CancellationToken ct)
        {
            // Central Error Notifier
            if (job.Context.Document is not null && _realTimeNotifier is not null)
                await _realTimeNotifier.NotifyFailureAsync(job.JobId, job.UserId, job.Context.Document, ex.Message, ct);
        }

        protected async Task NotifyProgress(EmbeddingJob job, ProcessDTO process, CancellationToken ct)
        {
            if (job.Context.Document is not null)
                await _realTimeNotifier.NotifyProgress(job.JobId, job.UserId, job.Context.Document, process, ct);
        }

        protected async Task NotifySuccesss(EmbeddingJob job, CancellationToken ct)
        {
            if (job.Context.Document is not null)
                await _realTimeNotifier.NotifySuccessAsync(job.JobId, job.UserId, job.Context.Document, ct);
        }
    }
}