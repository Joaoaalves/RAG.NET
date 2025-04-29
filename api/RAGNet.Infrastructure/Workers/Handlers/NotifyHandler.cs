using RAGNet.Domain.Services;
using RAGNET.Domain.Contexts;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Services.Queue;

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