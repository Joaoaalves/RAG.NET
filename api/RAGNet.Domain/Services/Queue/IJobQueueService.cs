using RAGNET.Domain.Entities.Jobs;

namespace RAGNET.Domain.Services.Queue
{
    public interface IJobQueueService
    {
        Task EnqueueAsync(EmbeddingJob job);
        Task<List<EmbeddingJob>> GetPendingJobsAsync();
        Task<EmbeddingJob?> DequeueAsync();
    }
}