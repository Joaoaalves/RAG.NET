using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.SharedKernel.Plans.Policies;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class NotifyHandler(
        ICallbackNotificationService<EmbeddingJobContext> callbackNotificationService,
        IJobNotificationService realTimeNotifier,
        SubscriptionPolicy subscriptionPolicy
    ) : NotifierJobProcessingHandler(realTimeNotifier)
    {
        public readonly ICallbackNotificationService<EmbeddingJobContext> _callbackNotificationService = callbackNotificationService;
        public readonly SubscriptionPolicy _subscriptionPolicy = subscriptionPolicy;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            if (job.Context.Document != null)
            {
                // Only send callback to ASCEND users
                if (_subscriptionPolicy.AllowsWebhookUsage(job.Context.User))
                    await _callbackNotificationService.NotifySuccessAsync(
                            job,
                            job.Context.TotalProcessed,
                            ct
                    );

                await NotifySuccesss(job, ct);
            }
        }
    }
}