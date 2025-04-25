using RAGNet.Domain.Services;
using RAGNET.Domain.Contexts;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Services.Queue;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class NotifyHandler(ICallbackNotificationService<EmbeddingJobContext> callbackNotificationService) : BaseJobProcessingHandler
    {
        public readonly ICallbackNotificationService<EmbeddingJobContext> _callbackNotificationService = callbackNotificationService;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            await _callbackNotificationService.NotifySuccessAsync(
                    job,
                    job.Context.TotalProcessed,
                    ct
                );
        }
    }
}