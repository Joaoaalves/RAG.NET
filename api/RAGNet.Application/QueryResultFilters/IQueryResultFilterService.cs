using RAGNET.Application.Providers.Conversation;
using RAGNET.Application.Queries;

namespace RAGNET.Application.QueryResultFilters
{
    public interface IQueryResultFilterService
    {
        Task<List<string>> FilterContent(List<ContentItem> contentItems, string query, IConversationProviderService completionProvider);
    }
}