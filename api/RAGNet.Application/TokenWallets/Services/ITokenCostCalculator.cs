using RAGNET.Application.Chunkers.Services;
using RAGNET.Application.Queries.DTOs;
using RAGNET.Application.Queries.Services;
using RAGNET.Application.QueryResultFilters.Services;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Application.TokenWallets.Services
{
    public interface ITokenCostCalculator
    {
        TokenAmount Calculate(IQueryEnhancerService queryEnhancer, int maxQueries);
        TokenAmount Calculate(IQueryResultFilterService queryResultFilterService, List<ContentItemDTO> contentItemDTOs);
        TokenAmount Calculate(ITextChunkerService chunkerService, int pages);
    }
}