using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.Commands.DeleteQueryResultFilter
{
    public class DeleteQueryResultFilterCommand(
        QueryResultFilterStrategy strategy
    ) : WorkflowAndUserAwareCommand<bool>
    {
        public QueryResultFilterStrategy Strategy { get; } = strategy;
    }
}