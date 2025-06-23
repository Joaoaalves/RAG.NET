using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Queries.FilterQueryResult
{
    public class FilterQueryResultCommand(
        List<ContentItem> items,
        string query
    ) : WorkflowAwareCommand<List<string>>
    {
        public List<ContentItem> Items { get; } = items;
        public string Query { get; } = query;
    }
}