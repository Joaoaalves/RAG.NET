using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.UpdateQueryResultFilter
{
    public class UpdateQueryResultFilterCommand(
        QueryResultFilterStrategy strategy,
        QueryResultFilterUpdateRequest data
    ) : WorkflowAndUserAwareCommand<QueryResultFilterDTO>
    {
        public QueryResultFilterUpdateRequest Data { get; } = data;
        public QueryResultFilterStrategy Strategy { get; } = strategy;
    }
}