namespace RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserTransactions
{
    public class GetUserTransactionsRequest
    {
        public DateTime? Start { get; set; } = null;
        public DateTime? End { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}