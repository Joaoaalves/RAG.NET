using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Shared;
using RAGNET.Application.TokenWallets.TokenTransactions.DTOs;

namespace RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserTransactions
{
    public class GetUserTransactionsQuery(
        GetUserTransactionsRequest request
    ) : UserAwareQuery<PagedResult<TokenTransactionDTO>>
    {
        public int Page { get; } = request.Page;
        public int PageSize { get; } = request.PageSize;
        public DateTime? Start { get; } = request.Start;
        public DateTime? End { get; } = request.End;
    }
}