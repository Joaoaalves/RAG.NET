using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.QueryResultFilters.DTOs;

namespace RAGNET.Application.QueryResultFilters.Commands.CreateQueryResultFilter
{
    public class CreateQueryResultFilterCommand(
        QueryResultFilterCreationRequest data
    ) : WorkflowAndUserAwareCommand<QueryResultFilterDTO>
    {
        public QueryResultFilterCreationRequest Data { get; } = data;
    }
}