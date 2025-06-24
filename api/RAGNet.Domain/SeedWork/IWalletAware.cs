using RAGNET.Domain.TokenWallets;

namespace RAGNET.Domain.SeedWork
{
    public interface IWalletAware
    {
        void InjectWallet(TokenWallet wallet);
    }
}