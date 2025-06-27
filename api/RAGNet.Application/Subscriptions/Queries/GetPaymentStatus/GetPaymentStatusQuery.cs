using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Subscriptions.Services;

namespace RAGNET.Application.Subscriptions.Queries.GetPaymentStatus
{
    public class GetPaymentStatusQuery(
        string paymentId
    ) : UserAwareQuery<PaymentStatus?>
    {
        public string PaymentId { get; } = paymentId;
    }
}