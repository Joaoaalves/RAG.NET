using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.TokenWallets.TokenTransactions;

namespace RAGNET.Application.TokenWallets.Queries.GetUserDailyTransactions
{
    public class GetUserDailyTransactionsQueryHandler(
        ITokenWalletRepository tokenWalletRepository
    ) : IQueryHandler<GetUserDailyTransactionsQuery, List<TransactionDailyAggregation>>
    {
        private readonly ITokenWalletRepository _tokenWalletRepository = tokenWalletRepository;

        public async Task<List<TransactionDailyAggregation>> Handle(GetUserDailyTransactionsQuery request, CancellationToken cancellationToken)
        {
            var start = request.Start.ToUniversalTime();
            var end = request.End.ToUniversalTime();

            return await _tokenWalletRepository.GetTransactionDailyAggregations(
                start,
                end,
                request.User.Id
            );
        }
    }
}