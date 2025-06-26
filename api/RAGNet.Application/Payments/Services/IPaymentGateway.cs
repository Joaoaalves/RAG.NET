namespace RAGNET.Application.Payments.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreateCheckoutSessionAsync(string userId, string successUrl, string cancelUrl, CancellationToken ct);
        Task<string?> ExtractUserIdFromEventAsync(string stripeEventJson, string stripeSignature);
    }
}