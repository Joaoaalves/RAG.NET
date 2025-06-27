namespace RAGNET.Infrastructure.Payments
{
    public class StripeSettings
    {
        public string SecretKey { get; init; } = string.Empty;
        public string WebhookSecret { get; init; } = string.Empty;
        public string PriceIdEnhanced { get; init; } = string.Empty;
        public string PriceIdAscend { get; init; } = string.Empty;
    }
}