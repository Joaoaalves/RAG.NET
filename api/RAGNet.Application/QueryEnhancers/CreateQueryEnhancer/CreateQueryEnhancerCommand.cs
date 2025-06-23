using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.QueryEnhancers.CreateQueryEnhancer
{
    public class CreateQueryEnhancerCommand(
            QueryEnhancerStrategy type,
            int maxQueries,
            string? prompt = null
    ) : WorkflowAndUserAwareCommand<QueryEnhancerDTO>
    {
        public QueryEnhancerStrategy Type { get; } = type;
        public string Prompt { get; } = prompt ?? String.Empty;
        public int MaxQueries { get; } = maxQueries;
    }
}