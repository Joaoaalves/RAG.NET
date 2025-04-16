using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;

namespace RAGNET.Application.UseCases.Query
{
    public interface IFilterContentUseCase
    {
        Task<List<string>> Execute(List<ContentItem> items, Workflow workflow, string query);
    }


    public class FilterContentUseCase
    (
        IChatCompletionFactory chatCompletionFactory,
        IContentFilterFactory contentFilterFactory
    ) : IFilterContentUseCase
    {
        public async Task<List<string>> Execute
        (
            List<ContentItem> items,
            Workflow workflow,
            string query
        )
        {
            if (workflow.Filter == null || !workflow.Filter.IsEnabled)
                return [];


            var completionProvider = chatCompletionFactory.CreateCompletionService(
                workflow.ConversationProviderConfig ??
                throw new ConversationProviderNotSetException(
                        "You must set your conversation provider before filtering."
                )
            );

            var contentFilterService = contentFilterFactory.CreateContentFilter(workflow.Filter);

            return await contentFilterService.FilterContent(items, query, completionProvider);
        }
    }
}