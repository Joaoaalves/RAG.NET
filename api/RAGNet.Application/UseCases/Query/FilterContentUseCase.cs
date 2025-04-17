using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Factories;

namespace RAGNET.Application.UseCases.Query
{
    public interface IFilterContentUseCase
    {
        Task<List<string>> Execute(List<ContentItem> items, Workflow workflow, string query, string userConversationProviderApiKey);
    }


    public class FilterContentUseCase
    (
        IChatCompletionFactory chatCompletionFactory,
        IContentFilterFactory contentFilterFactory
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

            if (workflow.ConversationProviderConfig == null)
                throw new ConversationProviderNotSetException("You must set your conversation provider before filtering.");

            var completionProvider = chatCompletionFactory.CreateCompletionService(
                userConversationProviderApiKey,
                workflow.ConversationProviderConfig
            );

            var contentFilterService = contentFilterFactory.CreateContentFilter(workflow.Filter);

            return await contentFilterService.FilterContent(items, query, completionProvider);
        }
    }
}