using RAGNET.Application.Payments.DTOs;

namespace RAGNET.Application.Payments.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreateCheckoutSessionAsync(string userId, string successUrl, string cancelUrl, CancellationToken ct);
        Task<PaymentIntentDTO?> ExtractUserIdFromEventAsync(string stripeEventJson, string stripeSignature);
    }
}