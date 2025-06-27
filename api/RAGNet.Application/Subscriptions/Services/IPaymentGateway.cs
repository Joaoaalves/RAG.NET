using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreateCheckoutSessionAsync(string userId, string successUrl, string cancelUrl, PlanType type, CancellationToken ct);
        Task<PaymentIntentDTO?> ExtractUserIdFromEventAsync(string stripeEventJson, string stripeSignature);
    }
}