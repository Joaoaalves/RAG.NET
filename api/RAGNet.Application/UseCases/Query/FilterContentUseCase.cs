using RAGNET.Domain.Workflows;

using RAGNET.Application.Providers;
using RAGNET.Application.UserQueries;
using RAGNET.Application.UserQueriesResultFilters;

namespace RAGNET.Application.UseCases.Query
{
    public interface IFilterContentUseCase
    {
        Task<List<string>> Execute(List<ContentItem> items, Workflow workflow, string query, string userConversationProviderApiKey);
    }


    public class FilterContentUseCase
    (
        IChatCompletionFactory chatCompletionFactory,
        IQueryResultFilterFactory QueryResultFilterFactory
    ) : IFilterContentUseCase
    {
        public async Task<List<string>> Execute(
            List<ContentItem> items,
            Workflow workflow,
            string query,
            string userConversationProviderApiKey
        )
        {
            if (workflow.QueryResultFilter == null || !workflow.QueryResultFilter.IsEnabled)
                return [];

            var completionProvider = chatCompletionFactory.CreateCompletionService(
                userConversationProviderApiKey,
                workflow.ConversationProviderConfig
            );

            var QueryResultFilterService = QueryResultFilterFactory.CreateQueryResultFilter(workflow.QueryResultFilter);

            return await QueryResultFilterService.FilterContent(items, query, completionProvider);
        }
    }

}