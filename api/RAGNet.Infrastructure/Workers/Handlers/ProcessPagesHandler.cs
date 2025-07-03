using System.Collections.Concurrent;

using RAGNET.Domain.Documents;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.Documents.Pages.Chunks;

using RAGNET.Application.ProviderApiKeys.Services;

using RAGNET.Infrastructure.Jobs;
using RAGNET.Infrastructure.Jobs.Queue;
using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Application.TokenWallets.Services.Consumption;
using RAGNET.Application.TokenWallets.Services.Consumption.Strategies;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class ProcessPagesHandler(
        IApiKeyResolverService apiKeyResolver,
        IEmbeddingProcessingService embeddingService,
        IJobNotificationService realTimeNotifier,
        ITokenConsumerContext tokenConsumerContext) : BaseJobProcessingHandler
    {
        private readonly IApiKeyResolverService _apiKeyResolver = apiKeyResolver;
        private readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        private readonly IJobNotificationService _realTimeNotifier = realTimeNotifier;

        private readonly ITokenConsumerContext _tokenConsumerContext = tokenConsumerContext;

        private readonly ProcessDTO _currentProcess = new()
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
            var wallet = job.Context.User.TokenWallet;

            var document = job.Context.Document ?? throw new Exception("Document is not set");

            await NotifyProgress(job, document, ct);

            var totalPages = document.Pages.Count;
            int processedPages = 0;

            var chunker = job.Context.TextChunkerService;

            var tokenConsumptionStrategy = new ChunkerConsumptionStrategy(
                workflow,
                chunker,
                totalPages
            );

            await _tokenConsumerContext.ConsumeAsync(
                workflow,
                wallet,
                tokenConsumptionStrategy,
                ct
            );

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
                        var results = await _embeddingService.GetEmbeddingsAsync(
                                          chunks,
                                          job.Context.EmbeddingProviderService
                                      );

                        var batch = results
                            .Select(r => (r.VectorId, r.Embedding, new Dictionary<string, string>()))
                            .ToList();

                        await _embeddingService.InsertEmbeddingBatchAsync(batch, workflow.CollectionId.ToString());

                        foreach (var (ChunkText, VectorId, Embedding) in results)
                        {
                            var chunk = Chunk.Create(
                                pageId: page.Id,
                                text: new Text(ChunkText),
                                vectorId: VectorId,
                                vector: new SemanticVector(Embedding)
                            );

                            chunksBag.Add(chunk);
                        }
                    }

                    var finished = Interlocked.Increment(ref processedPages);

                    _currentProcess.Progress = (int)(finished / (double)totalPages * 100);
                    await NotifyProgress(job, document, ct);

                    return chunks.Count;
                }
                catch (HttpRequestException)
                {
                    throw;
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    return 0;
                }

            }));

            await StoreVectors(chunksBag, job, document, ct);

            job.Context.TotalProcessed = counts.Sum();

            await base.HandleAsync(job, ct);
        }
    }
}