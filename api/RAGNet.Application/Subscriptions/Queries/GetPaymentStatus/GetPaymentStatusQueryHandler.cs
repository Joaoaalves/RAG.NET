using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Application.Subscriptions.Services;

namespace RAGNET.Application.Subscriptions.Queries.GetPaymentStatus
{
    public class GetPaymentStatusQueryHandler(
        IPaymentStatusService paymentStatusService
    ) : IQueryHandler<GetPaymentStatusQuery, PaymentStatus?>
    {

        private readonly IPaymentStatusService _paymentStatusService = paymentStatusService;
        public async Task<PaymentStatus?> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
        {
            var intent = new PaymentIntentDTO
            {
                UserId = request.User.Id,
                PaymentIntentId = request.PaymentId
            };

            var paymentStatus = await _paymentStatusService.GetPaymentIntent(intent);

            if (paymentStatus.HasValue && paymentStatus == PaymentStatus.SUCCESS)
            {
                await _paymentStatusService.RemovePaymentIntent(intent);
            }

            return paymentStatus;
        }
    }
}