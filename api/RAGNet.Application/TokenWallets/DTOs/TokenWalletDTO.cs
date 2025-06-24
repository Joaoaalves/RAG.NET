using RAGNET.Application.TokenWallets.TokenTransactions.DTOs;

namespace RAGNET.Application.TokenWallets.DTOs
{
    public class TokenWalletDTO
    {
        public decimal FreeTokens { get; set; }
        public decimal PaidTokens { get; set; }
        public DateTime LastFreeTokenResetAt { get; set; }
        public List<TokenTransactionDTO> Transactions { get; set; } = [];
    }
}