using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Application.TokenWallets.TokenTransactions.DTOs
{
    public class TokenTransactionDTO
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string OperationName { get; set; } = string.Empty;
        public string ContextInfo { get; set; } = string.Empty;
        public long Cost { get; set; }
        public DateTime TimeStamp { get; set; }
        public TokenSource Source { get; set; }
    }
}