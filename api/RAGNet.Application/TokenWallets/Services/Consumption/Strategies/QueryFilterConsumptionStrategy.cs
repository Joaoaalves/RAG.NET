using RAGNET.Application.Queries.DTOs;
using RAGNET.Application.QueryResultFilters.Services;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.TokenWallets.Services.Consumption.Strategies
{
    public class QueryFilterConsumptionStrategy(
        Workflow workflow,
        IQueryResultFilterService filterService,
        List<ContentItemDTO> contentItems
    ) : ITokenConsumptionStrategy
    {
        public string Operation => "Query Filter";
        private readonly int WordsPerPage = 300;
        public string GetContextInfo()
            => $"workflowId={workflow.Id.Value};QueryFilter={workflow.QueryResultFilter!.Strategy}";

        public string GetUserId() => workflow.UserId;

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