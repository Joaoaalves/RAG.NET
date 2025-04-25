using System.Collections.Concurrent;

using RAGNet.Domain.Services;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Entities.Jobs;

using RAGNET.Domain.Services;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Infrastructure.Workers.Handlers
{
    public class ProcessPagesHandler(IApiKeyResolverService apiKeyResolver, IEmbeddingProcessingService embeddingService) : BaseJobProcessingHandler
    {
        public readonly IApiKeyResolverService _apiKeyResolver = apiKeyResolver;
        public readonly IEmbeddingProcessingService _embeddingService = embeddingService;
        public override async Task HandleAsync(EmbeddingJob job, CancellationToken ct)
        {
            var workflow = job.Context.Workflow;

            var document = job.Context.Document ?? throw new Exception("Document is not set");

            var convoKey = await _apiKeyResolver.ResolveForUserAsync(
                job.UserId,
                workflow.ConversationProviderConfig.Provider);

            var embedKey = await _apiKeyResolver.ResolveForUserAsync(
                job.UserId,
                workflow.ConversationProviderConfig.Provider
            );

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

                    return chunks.Count;
                }
                catch
                {
                    return 0;
                }

            }));

            await _embeddingService.AddChunksAsync([.. chunksBag]);

            job.Context.TotalProcessed = counts.Sum();

            await base.HandleAsync(job, ct);
        }
    }
}