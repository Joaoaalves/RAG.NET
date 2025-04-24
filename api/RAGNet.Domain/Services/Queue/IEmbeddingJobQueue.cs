using RAGNET.Domain.Entities.Jobs;

namespace RAGNET.Domain.Services.Queue
{
    public interface IEmbeddingJobQueue
    {
        Task EnqueueAsync(EmbeddingJob job, CancellationToken cancellationToken);
    }
}