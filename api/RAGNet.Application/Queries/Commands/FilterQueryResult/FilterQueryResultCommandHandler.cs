using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Infrastructure.Providers.Conversation;
using RAGNET.Application.ProviderApiKeys.Services;
using RAGNET.Application.QueryResultFilters.Factories;

namespace RAGNET.Application.Queries.Commands.FilterQueryResult
{
    public class FilterQueryResultCommandHandler(
        IConversationProviderFactory chatCompletionFactory,
        IQueryResultFilterFactory queryResultFilterFactory,
        IApiKeyResolverService apiKeyResolverService
    ) : ICommandHandler<FilterQueryResultCommand, List<string>>
    {
        private readonly IConversationProviderFactory _chatCompletionFactory = chatCompletionFactory;
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