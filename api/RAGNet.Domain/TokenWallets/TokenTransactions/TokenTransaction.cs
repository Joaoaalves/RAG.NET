using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Domain.TokenWallets.TokenTransactions
{
    public class TokenTransaction : Entity, IUserOwned
    {
        public TokenTransactionId Id { get; private init; } = default!;
        public string UserId { get; set; } = string.Empty;
        public string OperationName { get; private set; } = string.Empty;
        public string ContextInfo { get; private set; } = string.Empty;
        public int Cost { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public TokenSource Source { get; private set; }
    }
}