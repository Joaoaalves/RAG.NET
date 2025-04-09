using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.Query
{
    public interface IQueryChunksUseCase
    {
        Task<List<Chunk>> Execute(string apiKey, List<string> queries, int topK);
    }

    public class QueryChunksUseCase(
        IVectorDatabaseService vectorDatabaseService,
        IQueryResultAggregatorService queryResultAggregatorService,
        IWorkflowRepository workflowRepository,
        IEmbedderFactory embedderFactory,
        IChunkRetrieverService chunkRetrieverService
    ) : IQueryChunksUseCase
    {
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IQueryResultAggregatorService _queryResultAggregatorService = queryResultAggregatorService;
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly IChunkRetrieverService _chunkRetrieverService = chunkRetrieverService;
        public async Task<List<Chunk>> Execute(string apiKey, List<string> queries, int topK)
        {
            try
            {
                // Setup
                var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey)
                    ?? throw new InvalidWorkflowApiKeyException("Invalid apikey.");

                var embConfig = workflow.EmbeddingProviderConfig
                    ?? throw new EmbeddingProviderNotSetException("You must set your embedding provider config");

                var embedderService = _embedderFactory.CreateEmbeddingService(embConfig);

                // Embedd All
                var embeddings = await embedderService.GetMultipleEmbeddingAsync(queries);
                var queryResults = await _vectorDatabaseService.QueryMultipleAsync
                (
                    embeddings,
                    workflow.CollectionId.ToString(),
                    topK
                );

                // Aggregate and rank topK results
                List<VectorQueryResult> aggregatedResults = _queryResultAggregatorService.AggregateResults(queryResults, topK);

                // Return chunks data with query scores.
                return await _chunkRetrieverService.RetrieveChunks(aggregatedResults);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw new Exception("An error occurred while querying on vector database");
            }
        }
    }
}
