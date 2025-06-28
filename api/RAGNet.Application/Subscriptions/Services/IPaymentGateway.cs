using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreateCustomerAsync(string userId, string firstName, string lastName, string email);
        Task<string> CreateCheckoutSessionAsync(string userId, string customerId, string successUrl, string cancelUrl, PlanType type, CancellationToken ct);
        Task ChangeSubscriptionPlanAsync(PlanType newPlanType, string subscriptionId, bool prorate);
        Task<PaymentIntentDTO?> ExtractUserIdFromEventAsync(string stripeEventJson, string stripeSignature);
    }
}