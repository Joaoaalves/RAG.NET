using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

using RAGNET.Application.Payments.Services;
using RAGNET.Application.Payments.DTOs;

namespace RAGNET.Infrastructure.Payments
{
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly StripeSettings _settings;

        public StripePaymentGateway(IOptions<StripeSettings> options)
        {
            _settings = options.Value;
            StripeConfiguration.ApiKey = _settings.SecretKey;
        }

        public async Task<string> CreateCheckoutSessionAsync(string userId, string successUrl, string cancelUrl, CancellationToken ct)
        {
            var options = new SessionCreateOptions
            {
                Mode = "subscription",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = [
                        new()
                        {
                            Price = _settings.PriceId,
                            Quantity = 1
                        }
                    ],
                Metadata = new Dictionary<string, string>
                    {
                        { "userId", userId },
                        { "paymentIntentId", Guid.NewGuid().ToString()}
                    }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options, cancellationToken: ct);

            return session.Url;
        }

        public Task<PaymentIntentDTO?> ExtractUserIdFromEventAsync(string json, string signature)
        {
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _settings.WebhookSecret);

            if (stripeEvent.Type == "checkout.session.completed" &&
                stripeEvent.Data.Object is Session session &&
                session.Metadata is not null &&
                session.Metadata.TryGetValue("userId", out var userId) &&
                session.Metadata.TryGetValue("paymentIntentId", out var paymentIntentId))
            {
                return Task.FromResult<PaymentIntentDTO?>(new PaymentIntentDTO { UserId = userId, PaymentIntentId = paymentIntentId });
            }

            return Task.FromResult<PaymentIntentDTO?>(null);
        }
    }
}