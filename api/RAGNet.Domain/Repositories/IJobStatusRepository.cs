using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Repositories
{
    public interface IJobStatusRepository
    {
        Task SetPendingAsync(Guid jobId);
        Task MarkAsCompletedAsync(Guid jobId);
        Task<JobStatus?> TryGetStatusAsync(Guid jobId);
    }
}