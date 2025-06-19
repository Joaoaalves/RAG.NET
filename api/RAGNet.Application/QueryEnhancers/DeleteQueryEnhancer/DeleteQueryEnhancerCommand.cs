using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryEnhancers.DeleteQueryEnhancer
{
    public class DeleteQueryEnhancerCommand(
        string userId,
        QueryEnhancerId queryEnhancerId
    ) : ICommand<bool>
    {
        public Guid Id { get; } = Guid.NewGuid(); public string UserId { get; } = userId;
        public QueryEnhancerId QueryEnhancerId { get; } = queryEnhancerId;
    }
}