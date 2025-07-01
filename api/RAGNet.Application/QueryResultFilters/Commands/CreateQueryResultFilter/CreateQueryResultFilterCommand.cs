using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.QueryResultFilters.DTOs;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.Commands.CreateQueryResultFilter
{
    public class CreateQueryResultFilterCommand(
        QueryResultFilterCreationRequest request,
        QueryResultFilterStrategy strategy
    ) : WorkflowAndUserAwareCommand<QueryResultFilterDTO>
    {
        public int MaxItems { get; set; } = request.MaxItems;
        public QueryResultFilterStrategy Strategy { get; set; } = strategy;
    }
}