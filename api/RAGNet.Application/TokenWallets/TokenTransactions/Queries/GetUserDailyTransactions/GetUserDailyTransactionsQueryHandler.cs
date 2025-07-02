using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserDailyTransactions
{
    public class GetUserDailyTransactionsQueryHandler(
        ITokenWalletRepository tokenWalletRepository
    ) : IQueryHandler<GetUserDailyTransactionsQuery, List<TransactionDailyAggregation>>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;

        public async Task<List<TransactionDailyAggregation>> Handle(GetUserDailyTransactionsQuery request, CancellationToken cancellationToken)
        {
            DateTime date = DateTime.UtcNow;
            DateTime startDate = request.Start ?? new DateTime(date.Year, date.Month, 1);
            DateTime endDate = request.End ?? date;

            var start = startDate.ToUniversalTime();
            var end = endDate.ToUniversalTime();

            return await _tokenWalletRepository.GetTransactionDailyAggregations(
                start,
                end,
                request.User.Id
            );
        }
    }
}