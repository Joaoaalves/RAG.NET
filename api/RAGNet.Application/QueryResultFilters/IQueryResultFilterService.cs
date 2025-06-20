using RAGNET.Application.Providers;
using RAGNET.Application.Queries;

namespace RAGNET.Application.UserQueriesResultFilters
{
    public interface IQueryResultFilterService
    {
        Task<List<string>> FilterContent(List<ContentItem> contentItems, string query, IChatCompletionService completionProvider);
    }
}