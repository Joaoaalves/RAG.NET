using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;

namespace RAGNET.Application.QueryEnhancers.UpdateQueryEnhancer
{
    public class UpdateQueryEnhancerCommand<TData>(
        string userId,
        QueryEnhancerId queryEnhancerId,
        TData data

    ) : ICommand<QueryEnhancerDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string UserId { get; } = userId;
        public QueryEnhancerId QueryEnhancerId { get; } = queryEnhancerId;
        public TData Data { get; } = data;
    }
}