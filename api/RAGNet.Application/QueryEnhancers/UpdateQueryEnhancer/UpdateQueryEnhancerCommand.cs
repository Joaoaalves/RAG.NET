using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;

namespace RAGNET.Application.QueryEnhancers.UpdateQueryEnhancer
{
    public class UpdateQueryEnhancerCommand(
        QueryEnhancerStrategy strategy,
        int maxQueries,
        bool? isEnabled = null,
        string? guidance = null

    ) : WorkflowAndUserAwareCommand<QueryEnhancerDTO>
    {
        public QueryEnhancerStrategy Strategy { get; } = strategy;
        public bool? IsEnabled { get; } = isEnabled;
        public int MaxQueries { get; } = maxQueries;
        public string? Guidance { get; } = guidance;
    }
}