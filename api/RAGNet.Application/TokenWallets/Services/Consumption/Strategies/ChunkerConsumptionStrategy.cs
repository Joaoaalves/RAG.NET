
using RAGNET.Application.Chunkers.Services;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption.Strategies
{
    public class ChunkerConsumptionStrategy(
           string userId,
           WorkflowId workflowId,
           ITextChunkerService chunkerService,
           string chunkerType,
           int pageCount
       ) : ITokenConsumptionStrategy
    {
        private readonly decimal CostPerPage = 0.02m;
        public string Operation => "Chunk";

        public string GetContextInfo()
            => $"workflowId={workflowId.Value};Chunker={chunkerType};pages={pageCount}";

        public string GetUserId() => userId;

        public TokenAmount CalculateCost()
        {
            decimal multiplier = chunkerService.GetCostMultiplier();
            decimal cost = multiplier * pageCount * CostPerPage;

            return TokenAmount.FromDecimal(cost);
        }
    }
}