using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.Query
{
    public interface IQueryChunksUseCase
    {
        Task<List<Chunk>> Execute(string apiKey, List<string> queries);
    }

    public class QueryChunksUseCase(
        IVectorDatabaseService vectorDatabaseService,
        IQueryResultAggregatorService queryResultAggregatorService,
        IWorkflowRepository workflowRepository,
        IEmbedderFactory embedderFactory,
        IChunkRepository chunkRepository
    ) : IQueryChunksUseCase
    {
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IQueryResultAggregatorService _queryResultAggregatorService = queryResultAggregatorService;
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        private readonly IChunkRepository _chunkRepository = chunkRepository;

        public async Task<List<Chunk>> Execute(string apiKey, List<string> queries)
        {
            try
            {
                // Setup
                var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey)
                    ?? throw new InvalidWorkflowApiKeyException("Invalid apikey.");

                var embConfig = workflow.EmbeddingProviderConfig
                    ?? throw new EmbeddingProviderNotSetException("You must set your embedding provider config");

                var embedderService = _embedderFactory.CreateEmbeddingService(embConfig.ApiKey, embConfig.Model, embConfig.Provider);

                // Exec query on vectorDB
                var tasks = queries.Select(async query =>
                {
                    var embedding = await embedderService.GetEmbeddingAsync(query);
                    return await _vectorDatabaseService.QueryAsync(embedding, workflow.CollectionId.ToString(), 5);
                });

                // Await all queries
                var queryResults = await Task.WhenAll(tasks);

                // Aggregate queryResult of every chunk
                var allResults = queryResults.SelectMany(qr => qr).ToList();

                // Aggregate and rank topK results
                VectorQueryResult?[] aggregatedResults = _queryResultAggregatorService.AggregateResults(allResults, 5);

                // For each aggregate result, search for the corresponding chunk
                var finalChunks = new List<Chunk>();
                foreach (var aggregatedResult in aggregatedResults)
                {
                    if (aggregatedResult != null)
                    {
                        var chunk = await _chunkRepository.GetByDocumentId(aggregatedResult.DocumentId);
                        if (chunk != null)
                            finalChunks.Add(chunk);
                    }
                }

                return finalChunks;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                throw new Exception("An error occurred while querying on vector database");
            }
        }
    }
}
