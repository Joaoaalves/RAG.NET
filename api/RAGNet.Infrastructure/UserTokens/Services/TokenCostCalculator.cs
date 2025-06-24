using RAGNET.Application.Queries.DTOs;
using RAGNET.Application.Queries.Services;
using RAGNET.Application.QueryResultFilters.Services;
using RAGNET.Application.TokenWallets.Services;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Infrastructure.UserTokens.Services
{
    public class TokenCostCalculator : ITokenCostCalculator
    {
        public readonly int WordsPerPage = 300;
        public TokenAmount Calculate(IQueryEnhancerService queryEnhancer, int maxQueries)
        {
            return TokenAmount.FromDecimal(queryEnhancer.GetCostMultiplier() * maxQueries);
        }

        public TokenAmount Calculate(IQueryResultFilterService queryResultFilterService, List<ContentItemDTO> contentItemDTOs)
        {
            int totalWords = contentItemDTOs.Sum(ci =>
                ci.Text?.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length ?? 0);

            int pages = (int)Math.Ceiling((double)totalWords / WordsPerPage);

            return TokenAmount.FromDecimal(queryResultFilterService.GetCostMultiplier() * pages);
        }
    }
}