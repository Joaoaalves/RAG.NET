using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.TokenWallets
{
    public class TokenWalletId : TypedIdValueBase
    {
        public TokenWalletId(Guid value) : base(value)
        {

        }

        public TokenWalletId() : base(Guid.NewGuid())
        {

        }
    }
}