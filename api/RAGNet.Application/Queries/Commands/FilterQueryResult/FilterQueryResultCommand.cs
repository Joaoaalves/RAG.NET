using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Queries.DTOs;

namespace RAGNET.Application.Queries.Commands.FilterQueryResult
{
    public class FilterQueryResultCommand(
        List<ContentItemDTO> items,
        string query
    ) : WorkflowAwareCommand<List<string>>
    {
        public List<ContentItemDTO> Items { get; } = items;
        public string Query { get; } = query;
    }
}