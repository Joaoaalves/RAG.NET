namespace RAGNET.Domain.TokenWallets.TokenTransactions
{
    public class TransactionDailyAggregation
    {
        public DateTime Date { get; set; }
        public long FreeTokensConsumed { get; set; }
        public long PaidTokensConsumed { get; set; }
    }
}