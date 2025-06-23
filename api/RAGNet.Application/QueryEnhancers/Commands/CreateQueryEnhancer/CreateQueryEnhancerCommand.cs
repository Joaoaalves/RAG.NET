using RAGNET.Domain.QueryEnhancers;

using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.QueryEnhancers.DTOs;

namespace RAGNET.Application.QueryEnhancers.Commands.CreateQueryEnhancer
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