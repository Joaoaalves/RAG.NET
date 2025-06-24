namespace RAGNET.Domain.TokenWallets
{
    public interface ITokenWalletRepository
    {
        Task<TokenWallet?> GetByUserIdAsync(string userId);
        Task UpdateAsync(TokenWallet wallet);
        Task DeleteAsync(TokenWallet wallet);
    }
}
