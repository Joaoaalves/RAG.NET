using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.ProviderApiKeys;
using RAGNET.Application.Providers;
using RAGNET.Application.UserQueriesResultFilters;

namespace RAGNET.Application.Queries.FilterQueryResult
{
    public class FilterQueryResultCommandHandler(
        IChatCompletionFactory chatCompletionFactory,
        IQueryResultFilterFactory queryResultFilterFactory,
        IApiKeyResolverService apiKeyResolverService
    ) : ICommandHandler<FilterQueryResultCommand, List<string>>
    {
        private readonly IChatCompletionFactory _chatCompletionFactory = chatCompletionFactory;
        private readonly IQueryResultFilterFactory _queryResultFilterFactory = queryResultFilterFactory;
        private readonly IApiKeyResolverService _apiKeyResolverService = apiKeyResolverService;
        public async Task<List<string>> Handle(
            FilterQueryResultCommand request,
            CancellationToken cancellationToken
        )
        {
            var workflow = request.Workflow;

            if (workflow.QueryResultFilter == null || !workflow.QueryResultFilter!.IsEnabled)
                return [];

            var userConversationProviderApiKey = await _apiKeyResolverService.ResolveForUserAsync(
                workflow.UserId,
                workflow.ConversationProviderConfig.Provider
            );

            var completionProvider = _chatCompletionFactory.CreateCompletionService(
                userConversationProviderApiKey,
                workflow.ConversationProviderConfig
            );

            var QueryResultFilterService = _queryResultFilterFactory
            .CreateQueryResultFilter(workflow.QueryResultFilter);

            return await QueryResultFilterService.FilterContent(
                request.Items,
                request.Query,
                completionProvider
            );
        }
    }
}