using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Mappers;
using RAGNET.Application.ProviderApiKeys;
using RAGNET.Application.Providers;
using RAGNET.Application.UserQueriesEnhancers;

namespace RAGNET.Application.QueryEnhancers.EnhanceQuery
{
    public class EnhanceQueryCommandHandler(
        IQueryEnhancerFactory queryEnhancerFactory,
        IChatCompletionFactory chatCompletionFactory,
        IApiKeyResolverService apiKeyResolverService
    ) : ICommandHandler<EnhanceQueryCommand, List<string>>
    {
        private readonly IQueryEnhancerFactory _queryEnhancerFactory = queryEnhancerFactory;
        private readonly IChatCompletionFactory _chatCompletionFactory = chatCompletionFactory;
        private readonly IApiKeyResolverService _apiKeyResolverService = apiKeyResolverService;
        public async Task<List<string>> Handle(EnhanceQueryCommand request, CancellationToken cancellationToken)
        {
            var workflow = request.Workflow;
            try
            {
                if (workflow.QueryEnhancers == null || workflow.QueryEnhancers.Count == 0)
                    return [
                        request.QueryDTO.Query
                    ];

                var userConversationProviderApiKey = await _apiKeyResolverService.ResolveForUserAsync(
                    workflow.UserId,
                    workflow.ConversationProviderConfig.Provider.ToSupportedProvider()
                );

                var completionService = _chatCompletionFactory.CreateCompletionService(
                    userConversationProviderApiKey,
                    workflow.ConversationProviderConfig
                );

                var tasks = workflow.QueryEnhancers.Select(async qeConfig =>
                {
                    if (qeConfig.IsEnabled)
                    {
                        var queryEnhancer = _queryEnhancerFactory.CreateQueryEnhancer(qeConfig, completionService);
                        return await queryEnhancer.GenerateQueries(request.QueryDTO.Query);
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