
using RAGNET.Application.Queries.Services;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption.Strategies
{
    public class QueryEnhancerConsumptionStrategy(
        string userId,
        WorkflowId workflowId,
        IQueryEnhancerService enhancer,
        string enhancerType,
        int maxQueries
    ) : ITokenConsumptionStrategy
    {
        public string Operation => "Query Enhancer";

        public string GetContextInfo()
            => $"workflowId={workflowId.Value};QueryEnhancer={enhancerType}";

        public string GetUserId() => userId;

        public TokenAmount CalculateCost()
            => TokenAmount.FromDecimal(enhancer.GetCostMultiplier() * maxQueries);
    }
}