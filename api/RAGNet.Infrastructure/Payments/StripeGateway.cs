using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

using RAGNET.Application.Subscriptions.Services;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Infrastructure.Payments
{
    public class StripeGateway : IPaymentGateway
    {
        private readonly StripeSettings _settings;


        public StripeGateway(IOptions<StripeSettings> options)
        {
            _settings = options.Value;
            StripeConfiguration.ApiKey = _settings.SecretKey;
        }

        public async Task<string> CreateCheckoutSessionAsync(string customerId, string successUrl, string cancelUrl, PlanType type, CancellationToken ct)
        {
            var options = new SessionCreateOptions
            {
                Mode = "subscription",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                Customer = customerId,
                LineItems = [
                        new()
                        {
                            Price = GetPriceId(type),
                            Quantity = 1
                        }
                    ],
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options, cancellationToken: ct);

            return session.Url;
        }

        public async Task<bool> CancelSubscriptionAsync(string subscriptionId)
        {
            var service = new SubscriptionService();

            Subscription subscription = await service.CancelAsync(subscriptionId);

            return subscription is not null;
        }

        public async Task ChangeSubscriptionPlanAsync(PlanType newPlanType, string subscriptionId, bool prorate)
        {
            var subscriptionService = new SubscriptionService();

            var subscription = await subscriptionService.GetAsync(subscriptionId);

            var subscriptionItemId = subscription.Items.Data.First().Id;

            var updateOptions = new SubscriptionItemUpdateOptions
            {
                Price = GetPriceId(newPlanType),
                ProrationBehavior = prorate ? "create_prorations" : "none"
            };

            var subscriptionItemService = new SubscriptionItemService();
            await subscriptionItemService.UpdateAsync(subscriptionItemId, updateOptions);
        }

        public async Task<string> CreateCustomerAsync(string userId, string firstName, string lastName, string email)
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
                Name = firstName + " " + lastName,
                Metadata = new Dictionary<string, string>{
                    {"userId", userId}
                }
            };

            var service = new CustomerService();
            var customer = await service.CreateAsync(options);

            return customer.Id;
        }

        public Task<PaymentIntentDTO?> ExtractPaymentIntentFromEventAsync(string json, string signature)
        {
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _settings.WebhookSecret);
            if (stripeEvent.Type == "invoice.payment_succeeded" &&
                stripeEvent.Data.Object is Invoice invoice)
            {
                var invoiceData = invoice.Lines.Data.First();
                var price = invoiceData.Pricing.PriceDetails.Price;
                var period = invoiceData.Period;

                return Task.FromResult<PaymentIntentDTO?>(new PaymentIntentDTO
                {
                    CustomerId = invoice.CustomerId,
                    PaymentIntentId = invoice.Id,
                    PlanType = GetPlanFromPriceId(price),
                    SubscriptionId = invoice.Parent.SubscriptionDetails.SubscriptionId,
                    RenewedAt = period.Start,
                    ExpiresAt = period.End
                });
            }

            return Task.FromResult<PaymentIntentDTO?>(null);
        }

        public Task<CancelSubscriptionIntentDTO?> ExtractCancelIntentFromEventAsync(string eventJson, string signature)
        {
            var stripeEvent = EventUtility.ConstructEvent(eventJson, signature, _settings.WebhookSecret);
            if (stripeEvent.Type == "customer.subscription.updated" &&
                stripeEvent.Data.Object is Subscription subscription &&
                subscription.CancelAt is not null)
            {
                return Task.FromResult<CancelSubscriptionIntentDTO?>(new CancelSubscriptionIntentDTO
                {
                    CustomerId = subscription.CustomerId,
                    SubscriptionId = subscription.Id,
                    CancelAt = subscription.CancelAt ?? DateTime.MinValue
                });
            }

            return Task.FromResult<CancelSubscriptionIntentDTO?>(null);
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

        private PlanType GetPlanFromPriceId(string priceId)
        {
            if (priceId == _settings.PriceIdEnhanced)
                return PlanType.Enhanced;

            if (priceId == _settings.PriceIdAscend)
                return PlanType.Ascend;

            throw new ArgumentOutOfRangeException("Invalid Price Id");
        }
    }
}