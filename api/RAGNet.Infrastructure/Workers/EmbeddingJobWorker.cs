using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Http.Internal;

using RAGNET.Domain.Entities;
using RAGNET.Domain.Entities.Jobs;

using RAGNET.Domain.Repositories;

using RAGNET.Domain.Factories;

using RAGNET.Domain.Services;
using RAGNET.Domain.Services.ApiKey;
using RAGNET.Domain.Services.Queue;

namespace RAGNET.Infrastructure.Workers
{
    public class EmbeddingJobWorker(
        IEmbeddingJobQueue jobQueue,
        IServiceScopeFactory scopeFactory
        ) : BackgroundService
    {
        private readonly IEmbeddingJobQueue _jobQueue = jobQueue;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

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

            var jobStatusRepo = scope.ServiceProvider.GetRequiredService<IJobStatusRepository>();
            var workflowRepo = scope.ServiceProvider.GetRequiredService<IWorkflowRepository>();
            var documentFactory = scope.ServiceProvider.GetRequiredService<IDocumentProcessorFactory>();
            var embeddingService = scope.ServiceProvider.GetRequiredService<IEmbeddingProcessingService>();
            var apiKeyResolver = scope.ServiceProvider.GetRequiredService<IApiKeyResolverService>();
            var callbackNotifier = scope.ServiceProvider.GetRequiredService<ICallbackNotificationService>();
            var httpClient = new HttpClient();

            try
            {
                await jobStatusRepo.SetPendingAsync(job.JobId);

                var tempFile = Path.Combine(Path.GetTempPath(), $"{job.JobId}_{job.FileName}");
                await File.WriteAllBytesAsync(tempFile, job.FileContent, ct);

                await using var fs = File.OpenRead(tempFile);
                var formFile = new FormFile(fs, 0, fs.Length, "file", job.FileName);

                var ext = Path.GetExtension(job.FileName).ToLowerInvariant();
                var processor = documentFactory.CreateDocumentProcessor(ext);
                var extract = await processor.ExtractTextAsync(formFile);
                var document = await processor.CreateDocumentWithPagesAsync(
                                    Path.GetFileNameWithoutExtension(job.FileName),
                                    Guid.Parse(job.WorkflowId.ToString()),
                                    extract.Pages
                                 );

                var workflow = await workflowRepo.DangerousGetById(job.WorkflowId) ?? throw new Exception("Workflow not found");

                var convoKey = await apiKeyResolver.ResolveForUserAsync(
                    workflow.UserId,
                    workflow.ConversationProviderConfig.Provider);

                var embedKey = await apiKeyResolver.ResolveForUserAsync(
                    workflow.UserId,
                    workflow.EmbeddingProviderConfig.Provider
                );

                var chunksBag = new ConcurrentBag<Chunk>();
                var counts = await Task.WhenAll(document.Pages.Select(async page =>
                {
                    var chunks = (await embeddingService.ChunkTextAsync(
                                     page.Text,
                                     workflow.Chunker,
                                     workflow.ConversationProviderConfig,
                                     convoKey
                                  )).ToList();

                    var results = await embeddingService.GetEmbeddingsAsync(
                                      chunks,
                                      workflow.EmbeddingProviderConfig,
                                      embedKey
                                  );

                    var batch = results
                        .Select(r => (r.VectorId, r.Embedding, new Dictionary<string, string>()))
                        .ToList();
                    await embeddingService.InsertEmbeddingBatchAsync(batch, workflow.CollectionId.ToString());

                    foreach (var (ChunkText, VectorId, Embedding) in results)
                        chunksBag.Add(new Chunk { PageId = page.Id, Text = ChunkText, VectorId = VectorId });

                    return chunks.Count;
                }));

                await embeddingService.AddChunksAsync([.. chunksBag]);


                // Update embedded documents count
                workflow.DocumentsCount++;
                await workflowRepo.UpdateByApiKey(workflow, workflow.ApiKey);
                await jobStatusRepo.MarkAsCompletedAsync(job.JobId);


                fs.Dispose();
                File.Delete(tempFile);

                // Notify all callback Urls
                var totalProcessed = counts.Sum();

                await callbackNotifier.NotifySuccessAsync(
                    job,
                    totalProcessed,
                    ct
                );
            }
            catch (Exception ex)
            {
                await callbackNotifier.NotifyFailureAsync(job, $"{ex.Message}", ct);
            }
        }
    }
}