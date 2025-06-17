using RAGNET.Application.DTOs.Query;
using RAGNET.Application.Providers;
using RAGNET.Application.QueryEnhancers;

using RAGNET.Domain.Workflows;

namespace RAGNET.Application.UseCases.QueryEnhancerUseCases
{
    public interface IEnhanceQueryUseCase
    {
        Task<List<string>> Execute(Workflow workflow, QueryDTO queryDTO, string userConversationProviderApiKey);
    }

    public class EnhanceQueryUseCase(
        IQueryEnhancerFactory queryEnhancerFactory,
        IChatCompletionFactory chatCompletionFactory
    ) : IEnhanceQueryUseCase
    {
        private readonly IQueryEnhancerFactory _queryEnhancerFactory = queryEnhancerFactory;
        private readonly IChatCompletionFactory _chatCompletionFactory = chatCompletionFactory;

        public async Task<List<string>> Execute(Workflow workflow, QueryDTO queryDTO, string userConversationProviderApiKey)
        {
            try
            {
                if (workflow.QueryEnhancers == null || workflow.QueryEnhancers.Count == 0)
                    return [];

                var completionService = _chatCompletionFactory.CreateCompletionService(
                    userConversationProviderApiKey,
                    workflow.ConversationProviderConfig
                );

                var tasks = workflow.QueryEnhancers.Select(async qeConfig =>
                {
                    if (qeConfig.IsEnabled)
                    {
                        var queryEnhancer = _queryEnhancerFactory.CreateQueryEnhancer(qeConfig, completionService);
                        return await queryEnhancer.GenerateQueries(queryDTO.Query);
                    }

                    return null;

                }).ToList();

                var results = await Task.WhenAll(tasks);
                var queries = results
                    .Where(result => result != null)
                    .SelectMany(result => result ?? [])
                    .ToList();
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
