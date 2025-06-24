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
        public TokenAmount Cost { get; private set; } = default!;
        public DateTime TimeStamp { get; private set; }
        public TokenSource Source { get; private set; }

        public TokenWalletId TokenWalletId { get; set; } = null!;
        public TokenWallet TokenWallet { get; set; } = null!;

        // EF Core ctor
        private TokenTransaction() { }

        private TokenTransaction(
            TokenTransactionId id,
            string userId,
            TokenWalletId tokenWalletId,
            string operationName,
            string contextInfo,
            TokenAmount cost,
            DateTime timeStamp,
            TokenSource source
        )
        {
            Id = id;
            UserId = userId;
            TokenWalletId = tokenWalletId;
            OperationName = operationName;
            ContextInfo = contextInfo;
            Cost = cost;
            TimeStamp = timeStamp;
            Source = source;
        }

        public static TokenTransaction Create(
            string userId,
            TokenWalletId tokenWalletId,
            string operationName,
            string contextInfo,
            TokenAmount cost,
            TokenSource source
        )
        {
            return new TokenTransaction(
                new TokenTransactionId(Guid.NewGuid()),
                userId,
                tokenWalletId,
                operationName,
                contextInfo,
                cost,
                DateTime.Now,
                source
            );
        }
    }
}