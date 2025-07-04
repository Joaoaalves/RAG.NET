using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class StoreVectorsHandler(
        IEmbeddingProcessingService embeddingService,
        IJobNotificationService realTimeNotifier
    ) : NotifierJobProcessingHandler(realTimeNotifier)
    {

        private readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        private readonly ProcessDTO _currentProcess = new()
        {
            Title = "Storing Vectors",
        };

        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var chunks = job.Context.Chunks;
            if (chunks is null || !chunks.Any())
            {
                throw new Exception("No chunks created");
            }

            await NotifyProgress(job, _currentProcess, ct);

            await _embeddingService.AddChunksAsync([.. chunks]);

            await base.HandleAsync(job, ct);
        }
    }
}