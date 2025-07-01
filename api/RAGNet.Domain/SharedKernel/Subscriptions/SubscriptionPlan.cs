using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Domain.SharedKernel.Subscriptions
{
    public class SubscriptionPlan : ValueObject
    {
        public PlanType Value { get; }

        public static readonly SubscriptionPlan Core = new(PlanType.Core);
        public static readonly SubscriptionPlan Ascend = new(PlanType.Ascend);
        public static readonly SubscriptionPlan Enhanced = new(PlanType.Enhanced);

        // EF Core
        private SubscriptionPlan() { }

        private SubscriptionPlan(PlanType type)
        {
            Value = type;
        }

        public TokenAmount GetTokenAllowance(BillingPeriod billingPeriod)
        {
            var strategy = TokenAllowanceStrategyFactory.GetStrategy(billingPeriod);
            return strategy.GetTokenAmount(Value);
        }

        public IEnumerable<ChunkerStrategy> AllowedChunkers => Value switch
        {
            PlanType.Core => [ChunkerStrategy.PARAGRAPH],
            PlanType.Ascend => [ChunkerStrategy.PARAGRAPH, ChunkerStrategy.PROPOSITION],
            PlanType.Enhanced => Enum.GetValues<ChunkerStrategy>(),
            _ => []
        };

        public bool Allows(ChunkerStrategy chunkerStrategy)
        {
            return AllowedChunkers.Contains(chunkerStrategy);
        }

        public static SubscriptionPlan FromType(PlanType type) => type switch
        {
            PlanType.Core => Core,
            PlanType.Ascend => Ascend,
            PlanType.Enhanced => Enhanced,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
