using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.QueryResultFilters.CreateQueryResultFilter
{
    public class CreateQueryResultFilterCommand(
        QueryResultFilterCreationRequest data
    ) : WorkflowAndUserAwareCommand<QueryResultFilterDTO>
    {
        public QueryResultFilterCreationRequest Data { get; } = data;
    }
}