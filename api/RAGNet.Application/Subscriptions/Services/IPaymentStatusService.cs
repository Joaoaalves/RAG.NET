using RAGNET.Application.Subscriptions.DTOs;

namespace RAGNET.Application.Subscriptions.Services
{
    public interface IPaymentStatusService
    {
        public Task<bool> CreatePaymentIntent(PaymentIntentDTO intent);
        public Task<PaymentStatus?> GetPaymentIntent(PaymentIntentDTO intent);
        public Task<PaymentStatus?> UpdatePaymentStatus(PaymentIntentDTO intent, PaymentStatus status);
        public Task<PaymentStatus?> RemovePaymentIntent(PaymentIntentDTO intent);
    }

    public enum PaymentStatus
    {
        SUCCESS,
        PENDING,
        CANCELED,
        ERROR
    }
}