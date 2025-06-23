using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryEnhancers.DeleteQueryEnhancer
{
    public class DeleteQueryEnhancerCommand(
        QueryEnhancerStrategy strategy
    ) : WorkflowAndUserAwareCommand<bool>
    {
        public QueryEnhancerStrategy Strategy { get; } = strategy;
    }
}