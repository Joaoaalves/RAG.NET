using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Contexts;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class NotifyHandler(ICallbackNotificationService<EmbeddingJobContext> callbackNotificationService, IJobNotificationService realTimeNotifier) : BaseJobProcessingHandler
    {
        public readonly ICallbackNotificationService<EmbeddingJobContext> _callbackNotificationService = callbackNotificationService;
        public readonly IJobNotificationService _realTimeNotifier = realTimeNotifier;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            if (job.Context.Document != null)
            {
                await _callbackNotificationService.NotifySuccessAsync(
                        job,
                        job.Context.TotalProcessed,
                        ct
                );

                await _realTimeNotifier.NotifySuccessAsync(job.JobId, job.UserId, job.Context.Document, ct);
            }

        }
    }
}