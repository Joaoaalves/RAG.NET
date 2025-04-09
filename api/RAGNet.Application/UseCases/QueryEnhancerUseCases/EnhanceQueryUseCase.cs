using RAGNET.Application.DTOs.Query;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface IEnhanceQueryUseCase
    {
        Task<List<string>> Execute(Workflow workflow, QueryDTO queryDTO);
    }

    public class EnhanceQueryUseCase(
        IQueryEnhancerFactory queryEnhancerFactory,
        IChatCompletionFactory chatCompletionFactory
    ) : IEnhanceQueryUseCase
    {
        private readonly IQueryEnhancerFactory _queryEnhancerFactory = queryEnhancerFactory;
        private readonly IChatCompletionFactory _chatCompletionFactory = chatCompletionFactory;

        public async Task<List<string>> Execute(Workflow workflow, QueryDTO queryDTO)
        {
            try
            {
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
