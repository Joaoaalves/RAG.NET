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
        IQueryResultFilterFactory contentFilterFactory
    ) : IFilterContentUseCase
    {
        public async Task<List<string>> Execute(
            List<ContentItem> items,
            Workflow workflow,
            string query,
            string userConversationProviderApiKey
        )
        {
            if (workflow.Filter == null || !workflow.Filter.IsEnabled)
                return [];

            var completionProvider = chatCompletionFactory.CreateCompletionService(
                userConversationProviderApiKey,
                workflow.ConversationProviderConfig
            );

            var contentFilterService = contentFilterFactory.CreateContentFilter(workflow.Filter);

            return await contentFilterService.FilterContent(items, query, completionProvider);
        }
    }

}