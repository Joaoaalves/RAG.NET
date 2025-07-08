using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using RAGNET.Infrastructure.Workers.Embedding.Handlers;
using RAGNET.Domain.SeedWork;
using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Contexts;

namespace RAGNET.Infrastructure.Workers.Embedding
{
    public class EmbeddingJobWorker(
        IEmbeddingJobQueue jobQueue,
        IServiceScopeFactory scopeFactory,
        ICallbackNotificationService<EmbeddingJobContext> callbackNotificationService
        ) : BackgroundService
    {
        private readonly IEmbeddingJobQueue _jobQueue = jobQueue;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ICallbackNotificationService<EmbeddingJobContext> _callbackNotificationService = callbackNotificationService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _jobQueue.SubscribeAsync(
                HandleJobAsync,
                autoAck: false,
                cancellationToken: stoppingToken
            ).ConfigureAwait(false);
        }

        private async Task HandleJobAsync(EmbeddingJob job, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            job.Context = new EmbeddingJobContext(scope);

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var initializeJobHandler = scope.ServiceProvider.GetRequiredService<InitializeJobHandler>();
            var mountProvidersHandler = scope.ServiceProvider.GetRequiredService<MountProvidersHandler>();
            var mountChunkerHandler = scope.ServiceProvider.GetRequiredService<MountChunkerHandler>();
            var mountVectorDatabaseHandler = scope.ServiceProvider.GetRequiredService<MountVectorDatabaseHandler>();
            var extractHandler = scope.ServiceProvider.GetRequiredService<ExtractTextHandler>();
            var consumeTokensHandler = scope.ServiceProvider.GetRequiredService<ConsumeTokensHandler>();
            var processPagesHandler = scope.ServiceProvider.GetRequiredService<ProcessPagesHandler>();
            var storeVectoresHandler = scope.ServiceProvider.GetRequiredService<StoreChunksHandler>();
            var updateWorkflowHandler = scope.ServiceProvider.GetRequiredService<UpdateWorkflowHandler>();
            var notifyHandler = scope.ServiceProvider.GetRequiredService<NotifyHandler>();

            initializeJobHandler.SetNext(extractHandler)
                .SetNext(mountProvidersHandler)
                .SetNext(mountChunkerHandler)
                .SetNext(mountVectorDatabaseHandler)
                .SetNext(consumeTokensHandler)
                .SetNext(processPagesHandler)
                .SetNext(storeVectoresHandler)
                .SetNext(updateWorkflowHandler)
                .SetNext(notifyHandler);
            try
            {
                await initializeJobHandler.HandleAsync(job, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                await _callbackNotificationService
                    .NotifyFailureAsync(job, ex.Message, ct);
                await unitOfWork.RevertAsync();
                throw;
            }
        }
    }
}