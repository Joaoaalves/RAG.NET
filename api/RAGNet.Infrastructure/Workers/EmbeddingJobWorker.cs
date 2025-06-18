using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


using RAGNET.Infrastructure.Workers.Handlers;
using RAGNET.Domain.SeedWork;
using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Infrastructure.Jobs.Contexts;
using RAGNET.Infrastructure.Jobs;

namespace RAGNET.Infrastructure.Workers
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
            var extractHandler = scope.ServiceProvider.GetRequiredService<ExtractTextHandler>();
            var processPagesHandler = scope.ServiceProvider.GetRequiredService<ProcessPagesHandler>();
            var updateWorkflowHanlder = scope.ServiceProvider.GetRequiredService<UpdateWorkflowHandler>();
            var notifyHandler = scope.ServiceProvider.GetRequiredService<NotifyHandler>();

            try
            {
                initializeJobHandler.SetNext(extractHandler);
                extractHandler.SetNext(processPagesHandler);
                processPagesHandler.SetNext(updateWorkflowHanlder);
                updateWorkflowHanlder.SetNext(notifyHandler);
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