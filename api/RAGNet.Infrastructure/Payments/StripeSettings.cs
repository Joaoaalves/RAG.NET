namespace RAGNET.Infrastructure.Payments
{
    public class StripeSettings
    {
        public string SecretKey { get; init; } = string.Empty;
        public string WebhookSecret { get; init; } = string.Empty;
        public PriceSettings Prices { get; init; } = new();

        public class PriceSettings
        {
            public PlanPrice Enhanced { get; init; } = new();
            public PlanPrice Ascend { get; init; } = new();
        }

        public class PlanPrice
        {
            public string Monthly { get; init; } = string.Empty;
            public string SemiAnnualy { get; init; } = string.Empty;
            public string Yearly { get; init; } = string.Empty;
        }
    }
}
