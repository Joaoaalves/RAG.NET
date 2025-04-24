using RAGNET.Domain.Entities.Jobs;

namespace RAGNET.Domain.Services.Queue
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