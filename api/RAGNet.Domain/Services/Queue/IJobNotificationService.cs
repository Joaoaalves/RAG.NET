using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services.Queue
{
    public interface IJobNotificationService
    {
        Task NotifyProgress(Guid jobId, string userId, Domain.Entities.Document document, CancellationToken ct = default);
        Task NotifySuccessAsync(Guid jobId, string userId, Document document, CancellationToken ct = default);
        Task NotifyFailureAsync(Guid jobId, Document document, string errorMessage, CancellationToken ct = default);
    }
}