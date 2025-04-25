using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using RAGNET.Domain.Entities.Jobs;


using RAGNET.Domain.Services.Queue;
using RAGNET.Domain.Contexts;
using RAGNET.Infrastructure.Workers.Handlers;

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
            try
            {
                Console.WriteLine("Running Worker");
                using var scope = _scopeFactory.CreateScope();

                var ctx = new EmbeddingJobContext(scope);
                job.Context = ctx;

                var initializeJobHandler = scope.ServiceProvider.GetRequiredService<InitializeJobHandler>();
                var extractHandler = scope.ServiceProvider.GetRequiredService<ExtractTextHandler>();
                var processPagesHandler = scope.ServiceProvider.GetRequiredService<ProcessPagesHandler>();
                var updateWorkflowHanlder = scope.ServiceProvider.GetRequiredService<UpdateWorkflowHandler>();
                var notifyHandler = scope.ServiceProvider.GetRequiredService<NotifyHandler>();


                initializeJobHandler.SetNext(extractHandler);
                extractHandler.SetNext(processPagesHandler);
                processPagesHandler.SetNext(updateWorkflowHanlder);
                updateWorkflowHanlder.SetNext(notifyHandler);

                await initializeJobHandler.HandleAsync(job, ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Received Exception {ex}");

                await _callbackNotificationService.NotifyFailureAsync(job, $"{ex.Message}", ct);
            }
        }
    }
}