namespace RAGNET.Domain.Services.Queue
{
    public interface IJobNotificationService
    {
        Task NotifySuccessAsync(Guid jobId, string userId, CancellationToken ct = default);
        Task NotifyFailureAsync(Guid jobId, string userId, string errorMessage, CancellationToken ct = default);
    }
}