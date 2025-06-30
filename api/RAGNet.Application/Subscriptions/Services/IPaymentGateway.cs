using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreateCustomerAsync(string userId, string firstName, string lastName, string email);
        Task<string> CreateCheckoutSessionAsync(string customerId, string successUrl, string cancelUrl, PlanType type, CancellationToken ct);
        Task ChangeSubscriptionPlanAsync(PlanType newPlanType, string subscriptionId, bool prorate);
        Task<bool> CancelSubscriptionAsync(string subscriptionId);
        Task<PaymentIntentDTO?> ExtractPaymentIntentFromEventAsync(string eventJson, string signature);
        Task<CancelSubscriptionIntentDTO?> ExtractCancelIntentFromEventAsync(string eventJson, string signature);
    }
}