using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserDailyTransactions
{
    public class GetUserDailyTransactionsQuery(
        GetUserDailyTransactionsRequest request
    ) : UserAwareQuery<List<TransactionDailyAggregation>>
    {
        public DateTime? Start { get; } = request.Start;
        public DateTime? End { get; } = request.End;
    }
}