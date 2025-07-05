using System.Collections.Concurrent;
using System.Text.Json;

using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.Documents.Pages.Chunks;

using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;

using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Infrastructure.Workers.Handlers
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

            var workflow = job.Context.Workflow;
            var chunker = job.Context.TextChunkerService;

            var document = job.Context.Document;
            var totalPages = document.Pages.Count;
            int processedPages = 0;

            var chunksBag = new ConcurrentBag<Chunk>();

            var counts = await Task.WhenAll(document.Pages.Select(async page =>
            {
                try
                {
                    var chunks = await _embeddingService.ChunkTextAsync(
                                                    chunker,
                                                    page.Text.Value
                                                 );
                    if (chunks.Count > 0)
                    {
                        var batch = await _embeddingService.GetEmbeddingsAsync(
                                          chunks,
                                          job.Context.EmbeddingProviderService
                                      );

                        await _embeddingService.InsertEmbeddingBatchAsync(batch, workflow.CollectionId.ToString());

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

                    _currentProcess.Progress = (int)(finished / (double)totalPages * 100);
                    await NotifyProgress(job, _currentProcess, ct);

                    return chunks.Count;
                }
                catch (JsonException)
                {
                    return 0;
                }
                catch (Exception)
                {
                    throw;
                }

            }));

            job.Context.TotalProcessed = counts.Sum();
            job.Context.Chunks = chunksBag;

            await base.HandleAsync(job, ct);
        }
    }
}