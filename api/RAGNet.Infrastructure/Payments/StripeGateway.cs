using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

using RAGNET.Application.Subscriptions.Services;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

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

        public async Task<string> CreateCheckoutSessionAsync(string userId, string successUrl, string cancelUrl, PlanType type, CancellationToken ct)
        {
            var options = new SessionCreateOptions
            {
                Mode = "subscription",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = [
                        new()
                        {
                            Price = GetPriceId(type),
                            Quantity = 1
                        }
                    ],
                Metadata = new Dictionary<string, string>
                    {
                        { "userId", userId },
                        { "paymentIntentId", Guid.NewGuid().ToString()},
                        { "planType", type.ToString()}
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
                session.Metadata is not null)
            {
                if (
                    session.Metadata.TryGetValue("userId", out var userId) &&
                    session.Metadata.TryGetValue("paymentIntentId", out var paymentIntentId) &&
                    session.Metadata.TryGetValue("planType", out var planType)
                )
                {
                    return Task.FromResult<PaymentIntentDTO?>(new PaymentIntentDTO { UserId = userId, PaymentIntentId = paymentIntentId, PlanType = planType });
                }
            }

            return Task.FromResult<PaymentIntentDTO?>(null);
        }

        private string GetPriceId(PlanType type)
        {
            return type switch
            {
                PlanType.Ascend => _settings.PriceIdAscend,
                PlanType.Enhanced => _settings.PriceIdEnhanced,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}