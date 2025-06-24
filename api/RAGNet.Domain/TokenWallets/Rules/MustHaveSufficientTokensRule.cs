using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Domain.TokenWallets.Rules
{
    public partial class MustHaveSufficientTokensRule(TokenWallet wallet, TokenAmount amount) : IBusinessRule
    {
        public string Message => "User does not have enough tokens to perform this operation.";

        public bool IsBroken() =>
            wallet.FreeTokens.Value + wallet.PaidTokens.Value < amount.Value;
    }
}