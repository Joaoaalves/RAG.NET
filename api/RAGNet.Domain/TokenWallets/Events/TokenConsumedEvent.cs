using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Domain.TokenWallets.Events
{
    public class TokenConsumedEvent : DomainEventBase
    {
        public string UserId { get; } = string.Empty;
        public string Operation { get; } = string.Empty;
        public string ContextInfo { get; } = String.Empty;
        public TokenAmount Amount { get; }
        public TokenSource Source { get; }
        public TokenWalletId TokenWalletId { get; }

        public TokenConsumedEvent(TokenWalletId tokenWalletId, string userId, TokenAmount amount, string operation, string contextInfo, TokenSource source)
        {
            UserId = userId;
            TokenWalletId = tokenWalletId;
            Operation = operation;
            ContextInfo = contextInfo;
            Amount = amount;
            Source = source;
        }
    }
}