using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.TokenWallets.TokenTransactions
{
    public class TokenTransactionId : TypedIdValueBase
    {
        public TokenTransactionId(Guid value) : base(value)
        {
        }
        public TokenTransactionId() : base(Guid.NewGuid())
        {
        }
    }
}