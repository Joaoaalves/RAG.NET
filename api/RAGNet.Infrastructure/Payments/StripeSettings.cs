namespace RAGNET.Infrastructure.Payments
{
    public class StripeSettings
    {
        public string SecretKey { get; init; } = string.Empty;
        public string WebhookSecret { get; init; } = string.Empty;
        public string PriceId { get; init; } = string.Empty;
    }
}