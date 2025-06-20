using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryResultFilters.CreateQueryResultFilter
{
    public class CreateQueryResultFilterCommand(
        WorkflowId workflowId,
        string userId,
        QueryResultFilter filter
    ) : ICommand<QueryResultFilterDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public WorkflowId WorkflowId { get; } = workflowId;
        public string UserId { get; } = userId;
        public QueryResultFilter Filter { get; } = filter;
    }
}