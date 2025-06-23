using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;

using RAGNET.Application.QueryResultFilters.DTOs;

namespace RAGNET.Application.QueryResultFilters.Commands.UpdateQueryResultFilter
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