namespace RAGNET.Domain.TokenWallets.TokenTransactions
{
    public class TransactionDailyAggregation
    {
        public DateTime Date { get; set; }
        public double FreeTokensConsumed { get; set; }
        public double PaidTokensConsumed { get; set; }
    }
}