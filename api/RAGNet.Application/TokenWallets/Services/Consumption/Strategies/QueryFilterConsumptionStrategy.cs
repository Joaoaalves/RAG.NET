using RAGNET.Application.Queries.DTOs;
using RAGNET.Application.QueryResultFilters.Services;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption.Strategies
{
    public class QueryFilterConsumptionStrategy(
        string userId,
        WorkflowId workflowId,
        IQueryResultFilterService filterService,
        string strategyName,
        List<ContentItemDTO> contentItems
    ) : ITokenConsumptionStrategy
    {
        public string Operation => "Query Filter";
        private readonly int WordsPerPage = 300;
        public string GetContextInfo()
            => $"workflowId={workflowId.Value};QueryFilter={strategyName}";

        public string GetUserId() => userId;

        public TokenAmount CalculateCost()
        {
            int totalWords = GetTotalWords();

            int pages = (int)Math.Ceiling((double)totalWords / WordsPerPage);

            decimal multiplier = filterService.GetCostMultiplier();
            decimal cost = multiplier * pages;

            return TokenAmount.FromDecimal(cost);
        }

        private int GetTotalWords()
        {
            return contentItems.Sum(ci =>
                ci.Text?.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length ?? 0);
        }
    }
}