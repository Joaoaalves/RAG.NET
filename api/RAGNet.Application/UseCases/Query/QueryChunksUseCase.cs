using RAGNET.Application.DTOs.Query;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Domain.Services.Query;

namespace RAGNET.Application.UseCases.Query
{
    public interface IQueryChunksUseCase
    {
        Task<List<ContentItem>> Execute(Workflow workflow, List<string> queries, QueryDTO queryDTO);
    }

    public class QueryChunksUseCase(
        IVectorDatabaseService vectorDatabaseService,
        IQueryResultAggregatorService queryResultAggregatorService,
        IEmbedderFactory embedderFactory,
        IScoreNormalizerService scoreNormalizerService,
        IChunkRetrieverService chunkRetrieverService
    ) : IQueryChunksUseCase
    {
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IQueryResultAggregatorService _queryResultAggregatorService = queryResultAggregatorService;
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly IScoreNormalizerService _scoreNormalizerService = scoreNormalizerService;
        private readonly IChunkRetrieverService _chunkRetrieverService = chunkRetrieverService;
        public async Task<List<ContentItem>> Execute(Workflow workflow, List<string> queries, QueryDTO queryDTO)
        {
            try
            {
                // Setup
                var embConfig = workflow.EmbeddingProviderConfig
                    ?? throw new EmbeddingProviderNotSetException("You must set your embedding provider config");

                var embedderService = _embedderFactory.CreateEmbeddingService(embConfig);

                // Embedd All
                var embeddings = await embedderService.GetMultipleEmbeddingAsync(queries);
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
