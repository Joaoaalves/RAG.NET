using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Application.TokenWallets.Queries.GetUserDailyTransactions
{
    public class GetUserDailyTransactionsQuery(
        DateTime start,
        DateTime end
    ) : UserAwareQuery<List<TransactionDailyAggregation>>
    {
        public DateTime Start { get; } = start;
        public DateTime End { get; } = end;
    }
}