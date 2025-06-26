using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Payments.Services;

namespace RAGNET.Application.Payments.Queries.GetPaymentStatus
{
    public class GetPaymentStatusQuery(
        string paymentId
    ) : UserAwareQuery<PaymentStatus?>
    {
        public string PaymentId { get; } = paymentId;
    }
}