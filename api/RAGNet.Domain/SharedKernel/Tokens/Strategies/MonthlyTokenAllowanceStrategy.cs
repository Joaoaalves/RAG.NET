using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Tokens.Strategies
{
    public class MonthlyTokenAllowanceStrategy : ITokenAllowanceStrategy
    {
        public TokenAmount GetTokenAmount(PlanType planType) => planType switch
        {
            PlanType.Core => TokenAmount.MonthlyFreeQuota,
            PlanType.Enhanced => TokenAmount.FromDecimal(3000),
            PlanType.Ascend => TokenAmount.FromDecimal(10000),
            _ => TokenAmount.Zero
        };
    }
}