
using RAGNET.Application.Chunkers.Services;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption.Strategies
{
    public class ChunkerConsumptionStrategy(
        Workflow workflow,
        ITextChunkerService chunkerService,
        int pageCount
    ) : ITokenConsumptionStrategy
    {
        private readonly decimal CostPerPage = 0.02m;
        public string Operation => "Chunk";

        public string GetContextInfo()
            => $"workflowId={workflow.Id.Value};Chunker={workflow.Chunker!.StrategyType};pages={pageCount}";

        public string GetUserId() => workflow.UserId;

        public TokenAmount CalculateCost()
        {
            decimal multiplier = chunkerService.GetCostMultiplier();
            decimal cost = multiplier * pageCount * CostPerPage;

            return TokenAmount.FromDecimal(cost);
        }
    }
}