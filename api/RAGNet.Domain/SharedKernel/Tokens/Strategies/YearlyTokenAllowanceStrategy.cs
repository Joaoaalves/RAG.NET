using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Tokens.Strategies
{
    public class YearlyTokenAllowanceStrategy : ITokenAllowanceStrategy
    {
        public TokenAmount GetTokenAmount(PlanType planType) => planType switch
        {
            PlanType.Core => TokenAmount.MonthlyFreeQuota,
            PlanType.Enhanced => TokenAmount.FromDecimal(36000),
            PlanType.Ascend => TokenAmount.FromDecimal(120000),
            _ => TokenAmount.Zero
        };
    }
}