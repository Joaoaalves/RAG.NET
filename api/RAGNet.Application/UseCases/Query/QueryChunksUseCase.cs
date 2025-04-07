using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.UseCases.Query
{
    public interface IQueryChunksUseCase
    {
        Task<Task<IEnumerable<VectorQueryResult>>[]?> Execute(string apiKey, List<string> queries);
    }

    public class QueryChunksUseCase(
        IVectorDatabaseService vectorDatabaseService,
        IWorkflowRepository workflowRepository,
        IEmbedderFactory embedderFactory
    ) : IQueryChunksUseCase
    {
        private readonly IVectorDatabaseService _vectorDatabaseService = vectorDatabaseService;
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IEmbedderFactory _embedderFactory = embedderFactory;
        public async Task<Task<IEnumerable<VectorQueryResult>>[]?> Execute(string apiKey, List<string> queries)
        {
            try
            {
                var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey) ?? throw new InvalidWorkflowApiKeyException("Invalid apikey.");

                var embConfig = workflow.EmbeddingProviderConfig ?? throw new EmbeddingProviderNotSetException("You must set your embedding provider config");
                var embedderService = _embedderFactory.CreateEmbeddingService(embConfig.ApiKey, embConfig.Model, embConfig.Provider);

                var tasks = queries.Select(async query =>
                {
                    var embedding = await embedderService.GetEmbeddingAsync(query);
                    return _vectorDatabaseService.QueryAsync(embedding, workflow.CollectionId.ToString(), 4);
                });

                var results = await Task.WhenAll(tasks);

                return results;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                throw new Exception("An error occurred while querying on vector database");
            }
        }
    }
}