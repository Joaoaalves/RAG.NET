using RAGNET.Application.TokenWallets.DTOs;
using RAGNET.Domain.TokenWallets;

namespace RAGNET.Application.TokenWallets.Mappers
{
    public static class TokenWalletMaper
    {
        public static TokenWalletDTO ToDTO(this TokenWallet tokenWallet)
        {
            return new TokenWalletDTO
            {
                FreeTokens = tokenWallet.FreeTokens.ToDecimal(),
                PaidTokens = tokenWallet.PaidTokens.ToDecimal(),
                LastFreeTokenResetAt = tokenWallet.LastFreeTokenResetAt
            };
        }
    }
}