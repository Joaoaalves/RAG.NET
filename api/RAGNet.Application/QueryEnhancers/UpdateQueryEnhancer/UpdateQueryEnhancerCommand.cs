using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;

namespace RAGNET.Application.QueryEnhancers.UpdateQueryEnhancer
{
    public class UpdateQueryEnhancerCommand(
        string userId,
        QueryEnhancerId queryEnhancerId,
        QueryEnhancerStrategy strategy,
        int maxQueries,
        bool? isEnabled = null,
        string? guidance = null

    ) : ICommand<QueryEnhancerDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string UserId { get; } = userId;
        public QueryEnhancerId QueryEnhancerId { get; } = queryEnhancerId;
        public QueryEnhancerStrategy Strategy { get; } = strategy;
        public bool? IsEnabled { get; } = isEnabled;
        public int MaxQueries { get; } = maxQueries;
        public string? Guidance { get; } = guidance;
    }
}