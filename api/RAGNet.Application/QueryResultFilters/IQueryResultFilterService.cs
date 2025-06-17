using RAGNET.Application.Providers;
using RAGNET.Application.Query;

namespace RAGNET.Application.QueryResultFilters
{
    public interface IQueryResultFilterService
    {
        Task<List<string>> FilterContent(List<ContentItem> contentItems, string query, IChatCompletionService completionProvider);
    }
}