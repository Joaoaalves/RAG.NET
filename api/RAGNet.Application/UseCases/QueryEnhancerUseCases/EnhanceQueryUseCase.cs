using RAGNET.Application.DTOs.Query;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface IEnhanceQueryUseCase
    {
        Task<List<string>> Execute(string apiKey, QueryDTO queryDTO);
    }

    public class EnhanceQueryUseCase(
        IWorkflowRepository workflowRepository,
        IQueryEnhancerFactory queryEnhancerFactory,
        IChatCompletionFactory chatCompletionFactory
    ) : IEnhanceQueryUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IQueryEnhancerFactory _queryEnhancerFactory = queryEnhancerFactory;
        private readonly IChatCompletionFactory _chatCompletionFactory = chatCompletionFactory;

        public async Task<List<string>> Execute(string apiKey, QueryDTO queryDTO)
        {
            try
            {
                var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey)
                    ?? throw new InvalidWorkflowApiKeyException("Api key is not valid!");

                var completionService = _chatCompletionFactory.CreateCompletionService(
                    workflow.ConversationProviderConfig ??
                    throw new ConversationProviderNotSetException("You must set your conversation provider before querying.")
                );

                var tasks = workflow.QueryEnhancers.Select(async qeConfig =>
                {
                    var queryEnhancer = _queryEnhancerFactory.CreateQueryEnhancer(qeConfig, completionService);
                    return await queryEnhancer.GenerateQueries(queryDTO.Query);
                }).ToList();

                var results = await Task.WhenAll(tasks);

                var queries = results.SelectMany(result => result).ToList();
                return queries;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                throw new Exception("An error occurred while processing your query");
            }
        }
    }
}
