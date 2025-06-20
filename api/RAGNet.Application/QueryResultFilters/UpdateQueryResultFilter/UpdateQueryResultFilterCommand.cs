using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryResultFilters;

namespace RAGNET.Application.QueryResultFilters.UpdateQueryResultFilter
{
    public class UpdateQueryResultFilterCommand(
        QueryResultFilterId filterId,
        string userId,
        QueryResultFilterUpdateRequest data
    ) : ICommand<QueryResultFilterDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public QueryResultFilterId FilterId { get; } = filterId;
        public QueryResultFilterUpdateRequest Data { get; } = data;
        public string UserId { get; } = userId;
    }
}