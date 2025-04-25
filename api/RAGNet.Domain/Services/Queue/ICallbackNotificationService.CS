using RAGNET.Domain.Entities.Jobs;

namespace RAGNET.Domain.Services.Queue
{
    public interface ICallbackNotificationService<TContext> where TContext : JobProcessingContext
    {
        public Task NotifySuccessAsync(
            Job<TContext> job,
            int processedChunks,
            CancellationToken cancellationToken = default);

        public Task NotifyFailureAsync(
            Job<TContext> job,
            string errorMessage,
            CancellationToken cancellationToken = default);
    }
}