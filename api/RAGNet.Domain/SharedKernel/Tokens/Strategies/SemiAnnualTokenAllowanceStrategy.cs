using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Tokens.Strategies
{
    public class SemiAnnualTokenAllowanceStrategy : ITokenAllowanceStrategy
    {
        public TokenAmount GetTokenAmount(PlanType planType) => planType switch
        {
            PlanType.Core => TokenAmount.MonthlyFreeQuota,
            PlanType.Enhanced => TokenAmount.FromDecimal(18000),
            PlanType.Ascend => TokenAmount.FromDecimal(60000),
            _ => TokenAmount.Zero
        };
    }
}