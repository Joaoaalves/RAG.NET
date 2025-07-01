using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Infrastructure.Jobs.Queue
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