namespace RAGNET.Application.TokenWallets.TokenTransactions.Queries.GetUserDailyTransactions
{
    public class GetUserDailyTransactionsRequest
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}