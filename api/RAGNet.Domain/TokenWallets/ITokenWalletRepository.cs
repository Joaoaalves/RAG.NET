namespace RAGNET.Domain.TokenWallets
{
    public interface ITokenWalletRepository
    {
        Task<TokenWallet?> GetByUserIdAsync(string userId);
        Task<TokenWallet> AddAsync(TokenWallet wallet);
        Task UpdateAsync(TokenWallet wallet);
        Task DeleteAsync(TokenWallet wallet);
    }
}
