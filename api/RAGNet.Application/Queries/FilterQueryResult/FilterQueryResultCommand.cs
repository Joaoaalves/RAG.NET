using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Queries.FilterQueryResult
{
    public class FilterQueryResultCommand(
        List<ContentItem> items,
        Workflow workflow,
        string query
    ) : ICommand<List<string>>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public List<ContentItem> Items { get; } = items;
        public Workflow Workflow { get; } = workflow;
        public string Query { get; } = query;
    }
}