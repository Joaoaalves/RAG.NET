using RAGNET.Domain.SharedKernel.Subscriptions;
using RAGNET.Domain.SharedKernel.Tokens.Strategies;

namespace RAGNET.Domain.SharedKernel.Tokens
{
    public static class TokenAllowanceStrategyFactory
    {
        public static ITokenAllowanceStrategy GetStrategy(BillingPeriod billingPeriod) => billingPeriod switch
        {
            BillingPeriod.Monthly => new MonthlyTokenAllowanceStrategy(),
            BillingPeriod.SemiAnnualy => new SemiAnnualTokenAllowanceStrategy(),
            BillingPeriod.Yearly => new YearlyTokenAllowanceStrategy(),
            _ => throw new ArgumentOutOfRangeException(nameof(billingPeriod), "Invalid billing period")
        };
    }

}