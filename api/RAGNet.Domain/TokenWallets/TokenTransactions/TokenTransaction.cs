using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

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

        public WorkflowId WorkflowId { get; set; } = null!;
        public TokenWalletId TokenWalletId { get; set; } = null!;
        public TokenWallet TokenWallet { get; set; } = null!;

        // EF Core ctor
        private TokenTransaction() { }

        private TokenTransaction(
            TokenTransactionId id,
            string userId,
            WorkflowId workflowId,
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
            WorkflowId = workflowId;
            TokenWalletId = tokenWalletId;
            OperationName = operationName;
            ContextInfo = contextInfo;
            Cost = cost;
            TimeStamp = timeStamp;
            Source = source;
        }

        public static TokenTransaction Create(
            string userId,
            WorkflowId workflowId,
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
                workflowId,
                tokenWalletId,
                operationName,
                contextInfo,
                cost,
                DateTime.UtcNow,
                source
            );
        }
    }
}