using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.DeleteQueryResultFilter
{
    public class DeleteQueryResultFilterCommand(
        QueryResultFilterId filterId,
        string userId
    ) : ICommand<bool>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public QueryResultFilterId FilterId { get; } = filterId;
        public string UserId { get; } = userId;
    }
}