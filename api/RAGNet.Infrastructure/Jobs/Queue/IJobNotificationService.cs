using RAGNET.Domain.Documents;

namespace RAGNET.Infrastructure.Jobs.Queue
{
    public interface IJobNotificationService
    {
        Task NotifyProgress(Guid jobId, string userId, Document document, ProcessDTO currentProcess, CancellationToken ct = default);
        Task NotifySuccessAsync(Guid jobId, string userId, Document document, CancellationToken ct = default);
        Task NotifyFailureAsync(Guid jobId, string userId, Document document, CancellationToken ct = default);
    }
}