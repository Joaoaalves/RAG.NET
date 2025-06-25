using RAGNET.Domain.TokenWallets.TokenTransactions;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.TokenWallets
{
    public interface ITokenWalletRepository
    {
        Task<TokenWallet?> GetByUserIdAsync(string userId);
        Task<List<TransactionDailyAggregation>> GetTransactionDailyAggregations(DateTime start, DateTime end, string userId);
        Task UpdateAsync(TokenWallet wallet);
        Task DeleteAsync(TokenWallet wallet);
    }
}
