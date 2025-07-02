using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob
{
    public interface IEmbeddingJobQueue
    {
        Task EnqueueAsync(EmbeddingJob job, CancellationToken cancellationToken);
        Task SubscribeAsync(
                   Func<EmbeddingJob, CancellationToken, Task> handle,
                   bool autoAck = false,
                   CancellationToken cancellationToken = default
               );
        ValueTask DisposeAsync();
    }
}