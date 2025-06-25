
using RAGNET.Application.Queries.Services;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption.Strategies
{
    public class QueryEnhancerConsumptionStrategy(
        Workflow workflow,
        QueryEnhancerStrategy strategy,
        IQueryEnhancerService enhancer,
        int maxQueries
    ) : ITokenConsumptionStrategy
    {
        public string Operation => "Query Enhancer";

        public string GetContextInfo()
            => $"QueryEnhancer={strategy}";

        public string GetUserId() => workflow.UserId;

        public TokenAmount CalculateCost()
            => TokenAmount.FromDecimal(enhancer.GetCostMultiplier() * maxQueries);
    }
}