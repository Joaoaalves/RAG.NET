namespace RAGNET.Infrastructure.Jobs.Queue
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