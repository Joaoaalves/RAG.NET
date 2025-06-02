using RAGNET.Domain.Documents;
using RAGNET.Domain.Entities.Jobs;

namespace RAGNET.Domain.Services.Queue
{
    public interface IJobNotificationService
    {
        Task NotifyProgress(Guid jobId, string userId, Document document, Process currentProcess, CancellationToken ct = default);
        Task NotifySuccessAsync(Guid jobId, string userId, Document document, CancellationToken ct = default);
        Task NotifyFailureAsync(Guid jobId, string userId, Document document, CancellationToken ct = default);
    }
}