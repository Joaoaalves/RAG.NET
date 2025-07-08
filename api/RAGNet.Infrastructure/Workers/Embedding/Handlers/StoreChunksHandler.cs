using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class StoreChunksHandler(
        IChunkRepository chunkRepository,
        IJobNotificationService realTimeNotifier
    ) : NotifierJobProcessingHandler(realTimeNotifier)
    {

        private readonly IChunkRepository _chunkRepository = chunkRepository;
        private readonly ProcessDTO _currentProcess = new()
        {
            Title = "Storing Chunks",
        };

        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var chunks = job.Context.Chunks;

            if (chunks is null || !chunks.Any())
            {
                throw new Exception("No chunks created");
            }

            await NotifyProgress(job, _currentProcess, ct);

            await _chunkRepository.AddManyAsync([.. chunks]);

            await base.HandleAsync(job, ct);
        }
    }
}