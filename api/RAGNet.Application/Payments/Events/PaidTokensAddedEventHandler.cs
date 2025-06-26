using RAGNET.Application.Payments.DTOs;
using RAGNET.Application.Payments.Services;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.TokenWallets.Events;

namespace RAGNET.Application.Payments.Events
{
    public class PaidTokensAddedEventHandler(
        IPaymentStatusService paymentStatusService
    ) : INotificationHandler<PaidTokensAddedEvent>
    {
        private readonly IPaymentStatusService _paymentStatusService = paymentStatusService;
        public async Task Handle(PaidTokensAddedEvent notification, CancellationToken cancellationToken)
        {
            var intent = new PaymentIntentDTO
            {
                UserId = notification.UserId,
                PaymentIntentId = notification.PaymentId
            };

            await _paymentStatusService.UpdatePaymentStatus(intent, PaymentStatus.SUCCESS);
        }

    }
}