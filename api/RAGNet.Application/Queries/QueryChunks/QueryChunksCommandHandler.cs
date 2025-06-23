using RAGNET.Application.Chunkers;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.ProviderApiKeys;
using RAGNET.Application.Providers;
using RAGNET.Application.Providers.Embedding;

namespace RAGNET.Application.Queries.QueryChunks
{
    public class QueryChunksCommandHandler(
        IVectorDatabaseService vectorDatabaseService,
        IQueryResultAggregatorService queryResultAggregatorService,
        IEmbedderFactory embedderFactory,
        IScoreNormalizerService scoreNormalizerService,
        IChunkRetrieverService chunkRetrieverService,
        IApiKeyResolverService apiKeyResolverService
    ) : ICommandHandler<QueryChunksCommand, List<ContentItem>>
    {
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IQueryResultAggregatorService _queryResultAggregatorService = queryResultAggregatorService;
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly IScoreNormalizerService _scoreNormalizerService = scoreNormalizerService;
        private readonly IChunkRetrieverService _chunkRetrieverService = chunkRetrieverService;
        private readonly IApiKeyResolverService _apiKeyResolverService = apiKeyResolverService;
        public async Task<List<ContentItem>> Handle(QueryChunksCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = request.Workflow;
                var queryDTO = request.QueryDTO;
                var embConfig = workflow.EmbeddingProviderConfig;

                var userEmbeddingProviderApiKey = await _apiKeyResolverService.ResolveForUserAsync(
                    workflow.UserId,
                    workflow.EmbeddingProviderConfig.Provider.ToSupportedProvider()
                );

                var embedderService = _embedderFactory.CreateEmbeddingService(userEmbeddingProviderApiKey, embConfig);

                // Embedd All
                var embeddings = await embedderService.GetMultipleEmbeddingAsync(request.Queries);

                var queryResults = await _vectorDatabaseService.QueryMultipleAsync
                (
                    embeddings,
                    workflow.CollectionId.ToString(),
                    queryDTO.TopK
                );

                // Aggregate and rank topK results
                List<VectorQueryResult> aggregatedResults = _queryResultAggregatorService.AggregateResults(queryResults,
                    queryDTO.MinScore,
                    queryDTO.TopK);

                // Normalize scores
                aggregatedResults = _scoreNormalizerService.MaybeNormalizeScores
                (
                    aggregatedResults,
                    queryDTO.NormalizeScore,
                    queryDTO.MinNormalizedScore
                );

                // Return chunks data with query scores.
                // If Parent-Child technique is enabled, retrieve the entire parent content.
                return await _chunkRetrieverService.RetrieveContent(aggregatedResults, queryDTO.ParentChild);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw new Exception("An error occurred while querying on vector database");
            }
        }
    }
}