using System.Collections.Concurrent;

using RAGNet.Domain.Services;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Entities.Jobs;

using RAGNET.Domain.Services;
using RAGNET.Domain.Services.ApiKey;
using RAGNET.Domain.Services.Queue;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class ProcessPagesHandler(IApiKeyResolverService apiKeyResolver, IEmbeddingProcessingService embeddingService, IJobNotificationService realTimeNotifier) : BaseJobProcessingHandler
    {
        public readonly IApiKeyResolverService _apiKeyResolver = apiKeyResolver;
        public readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        public readonly IJobNotificationService _realTimeNotifier = realTimeNotifier;

        private readonly Process _currentProcess = new()
        {
            Title = "Embedding Extracted Text"
        };

        private async Task NotifyProgress(EmbeddingJob job, Document document, CancellationToken ct)
        {
            await _realTimeNotifier.NotifyProgress(job.JobId, job.UserId, document, _currentProcess, ct);
        }

        private async Task StoreVectors(ConcurrentBag<Chunk> chunksBag, EmbeddingJob job, Document document, CancellationToken ct)
        {
            _currentProcess.Title = "Storing Vectors";
            _currentProcess.Progress = 0;
            await NotifyProgress(job, document, ct);
            await _embeddingService.AddChunksAsync([.. chunksBag]);
            _currentProcess.Title = "Storing Vectors";
            _currentProcess.Progress = 100;
            await NotifyProgress(job, document, ct);

        }


        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var workflow = job.Context.Workflow;

            var document = job.Context.Document ?? throw new Exception("Document is not set");
            await NotifyProgress(job, document, ct);

            var convoKey = await _apiKeyResolver.ResolveForUserAsync(
                job.UserId,
                workflow.ConversationProviderConfig.Provider);

            var embedKey = await _apiKeyResolver.ResolveForUserAsync(
                job.UserId,
                workflow.ConversationProviderConfig.Provider
            );

            var totalPages = document.Pages.Count;
            int processedPages = 0;

            var chunksBag = new ConcurrentBag<Chunk>();
            var counts = await Task.WhenAll(document.Pages.Select(async page =>
            {
                try
                {
                    var chunks = (await _embeddingService.ChunkTextAsync(
                                                    page.Text,
                                                    workflow.Chunker,
                                                    workflow.ConversationProviderConfig,
                                                    convoKey
                                                 )).ToList();
                    if (chunks.Count > 0)
                    {
                        var results = await _embeddingService.GetEmbeddingsAsync(
                                          chunks,
                                          workflow.EmbeddingProviderConfig,
                                          embedKey
                                      );

                        var batch = results
                            .Select(r => (r.VectorId, r.Embedding, new Dictionary<string, string>()))
                            .ToList();

                        await _embeddingService.InsertEmbeddingBatchAsync(batch, workflow.CollectionId.ToString());

                        foreach (var (ChunkText, VectorId, Embedding) in results)
                            chunksBag.Add(new Chunk { PageId = page.Id, Text = ChunkText, VectorId = VectorId });
                    }

                    var finished = Interlocked.Increment(ref processedPages);

                    _currentProcess.Progress = (int)(finished / (double)totalPages * 100);
                    await NotifyProgress(job, document, ct);

                    return chunks.Count;
                }
                catch
                {
                    return 0;
                }

            }));

            await StoreVectors(chunksBag, job, document, ct);

            job.Context.TotalProcessed = counts.Sum();

            await base.HandleAsync(job, ct);
        }
    }
}