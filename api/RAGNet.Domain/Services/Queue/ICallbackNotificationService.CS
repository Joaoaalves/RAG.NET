using RAGNET.Domain.Entities.Jobs;

namespace RAGNET.Domain.Services.Queue
{
    public interface ICallbackNotificationService
    {
        public Task NotifySuccessAsync(
            Job job,
            int processedChunks,
            CancellationToken cancellationToken = default);

        public Task NotifyFailureAsync(
            Job job,
            string errorMessage,
            CancellationToken cancellationToken = default);
    }
}