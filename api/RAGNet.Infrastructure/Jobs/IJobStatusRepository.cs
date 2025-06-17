namespace RAGNET.Infrastructure.Jobs
{
    public interface IJobStatusRepository
    {
        Task SetPendingAsync(Guid jobId);
        Task MarkAsCompletedAsync(Guid jobId);
        Task<JobStatus?> TryGetStatusAsync(Guid jobId);
    }
}