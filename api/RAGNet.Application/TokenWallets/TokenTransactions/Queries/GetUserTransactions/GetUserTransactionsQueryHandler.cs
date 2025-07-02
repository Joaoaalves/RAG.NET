using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Shared;
using RAGNET.Application.TokenWallets.TokenTransactions.DTOs;
using RAGNET.Application.TokenWallets.TokenTransactions.Mappers;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserTransactions
{
    public class GetUserTransactionsQueryHandler(
        ITokenWalletRepository tokenWalletRepository
    ) : IQueryHandler<GetUserTransactionsQuery, PagedResult<TokenTransactionDTO>>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;
        public async Task<PagedResult<TokenTransactionDTO>> Handle(GetUserTransactionsQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var start = request.Start ?? new DateTime(now.Year, now.Month, 1);
            var end = request.End ?? now;

            var (transactions, count) = await _tokenWalletRepository.GetPagedTransactionsAsync(
                request.User.Id,
                request.Page,
                request.PageSize,
                start,
                end
            );

            return new PagedResult<TokenTransactionDTO>(transactions.ToDTOList(), count);
        }
    }
}