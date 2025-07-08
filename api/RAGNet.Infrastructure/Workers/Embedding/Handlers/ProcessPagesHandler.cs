using System.Collections.Concurrent;
using System.Text.Json;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Infrastructure.Workers.Embedding.Handlers
{
    public class ProcessPagesHandler(
        IEmbeddingProcessingService embeddingService,
        IJobNotificationService realTimeNotifier
    ) : NotifierJobProcessingHandler(realTimeNotifier)
    {
        private readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        private readonly ProcessDTO _currentProcess = new()
        {
            Title = "Embedding Extracted Text",
            Progress = 0
        };

        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            await NotifyProgress(job, _currentProcess, ct);

            var (workflow, chunker, document) = job.Context;
            var vectorDatabaseService = job.Context.VectorDatabaseService;

            var totalPages = document.Pages.Count;

            int processedPages = 0;
            var chunksBag = new ConcurrentBag<Chunk>();

            // Control max parallelism degree to avoid resource exhaustion
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 250,
                CancellationToken = ct
            };

            await Parallel.ForEachAsync(document.Pages, parallelOptions, async (page, token) =>
            {
                try
                {
                    var chunks = await _embeddingService.ChunkTextAsync(chunker, page.Text.Value);

                    if (chunks.Count > 0)
                    {
                        var batch = await _embeddingService.GetEmbeddingsAsync(chunks, job.Context.EmbeddingProviderService);

                        await vectorDatabaseService.InsertManyAsync(batch, workflow.CollectionId.ToString());

                        // Create chunks entities
                        foreach (var embedding in batch)
                        {
                            var chunk = Chunk.Create(
                                pageId: page.Id,
                                text: new Text(embedding.ChunkText),
                                vectorId: embedding.VectorId,
                                vector: embedding.Vector
                            );

                            chunksBag.Add(chunk);
                        }
                    }

                    var finished = Interlocked.Increment(ref processedPages);

                    // Update progress only every 5 pages to reduce notification noise
                    if (finished % 5 == 0 || finished == totalPages)
                    {
                        _currentProcess.Progress = (int)(finished / (double)totalPages * 100);
                        await NotifyProgress(job, _currentProcess, token);
                    }
                }
                catch (JsonException)
                {
                    Console.WriteLine($"JSON parsing failed for page {page.Id} in job {job.JobId}");
                    // Continue processing other pages
                }
                catch (Exception)
                {
                    Console.WriteLine($"Unexpected error processing page {page.Id} in job {job.JobId}");
                    throw; // escalate error to stop pipeline if necessary
                }
            });

            job.Context.TotalProcessed = chunksBag.Count;
            job.Context.Chunks = chunksBag;

            await base.HandleAsync(job, ct);
        }
    }
}
