using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.TokenWallets.Events
{
    public class TokenConsumedEvent : DomainEventBase
    {
        public string UserId { get; } = string.Empty;
        public WorkflowId WorkflowId { get; }
        public string Operation { get; } = string.Empty;
        public string ContextInfo { get; } = String.Empty;
        public TokenAmount Amount { get; }
        public TokenSource Source { get; }
        public TokenWalletId TokenWalletId { get; }

        public TokenConsumedEvent(TokenWalletId tokenWalletId, string userId, WorkflowId workflowId, TokenAmount amount, string operation, string contextInfo, TokenSource source)
        {
            UserId = userId;
            WorkflowId = workflowId;
            TokenWalletId = tokenWalletId;
            Operation = operation;
            ContextInfo = contextInfo;
            Amount = amount;
            Source = source;
        }
    }
}