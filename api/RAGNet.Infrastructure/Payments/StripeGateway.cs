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

        public async Task<string> CreateCheckoutSessionAsync(string customerId, string successUrl, string cancelUrl, PlanType type, BillingPeriod period, CancellationToken ct)
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
                            Price = GetPriceId(type, period),
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
                    BillingPeriod = GetBillingPeriodFromPriceId(price),
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

        private string GetPriceId(PlanType type, BillingPeriod period = BillingPeriod.Monthly)
        {
            return (type, period) switch
            {
                (PlanType.Enhanced, BillingPeriod.Monthly) => _settings.Prices.Enhanced.Monthly,
                (PlanType.Enhanced, BillingPeriod.SemiAnnualy) => _settings.Prices.Enhanced.SemiAnnualy,
                (PlanType.Enhanced, BillingPeriod.Yearly) => _settings.Prices.Enhanced.Yearly,
                (PlanType.Ascend, BillingPeriod.Monthly) => _settings.Prices.Ascend.Monthly,
                (PlanType.Ascend, BillingPeriod.SemiAnnualy) => _settings.Prices.Ascend.SemiAnnualy,
                (PlanType.Ascend, BillingPeriod.Yearly) => _settings.Prices.Ascend.Yearly,
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid plan or period")
            };
        }

        private PlanType GetPlanFromPriceId(string priceId)
        {
            if (priceId == _settings.Prices.Enhanced.Monthly ||
                priceId == _settings.Prices.Enhanced.SemiAnnualy ||
                priceId == _settings.Prices.Enhanced.Yearly)
            {
                return PlanType.Enhanced;
            }

            if (priceId == _settings.Prices.Ascend.Monthly ||
                priceId == _settings.Prices.Ascend.SemiAnnualy ||
                priceId == _settings.Prices.Ascend.Yearly)
            {
                return PlanType.Ascend;
            }

            throw new ArgumentOutOfRangeException(nameof(priceId), priceId, "Invalid Price Id");
        }

        private BillingPeriod GetBillingPeriodFromPriceId(string priceId)
        {
            if (priceId == _settings.Prices.Enhanced.Monthly || priceId == _settings.Prices.Ascend.Monthly)
                return BillingPeriod.Monthly;

            if (priceId == _settings.Prices.Enhanced.SemiAnnualy || priceId == _settings.Prices.Ascend.SemiAnnualy)
                return BillingPeriod.SemiAnnualy;

            if (priceId == _settings.Prices.Enhanced.Yearly || priceId == _settings.Prices.Ascend.Yearly)
                return BillingPeriod.Yearly;

            throw new ArgumentOutOfRangeException(nameof(priceId), priceId, "Invalid Price Id");
        }
    }
}