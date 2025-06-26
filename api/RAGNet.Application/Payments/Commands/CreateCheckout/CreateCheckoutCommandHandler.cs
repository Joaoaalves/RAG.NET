using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Payments.DTOs;
using RAGNET.Application.Payments.Services;

namespace RAGNET.Application.Payments.Commands.CreateCheckout
{
    public class CreateCheckoutCommandHandler(IPaymentGateway paymentGateway, IPaymentStatusService paymentStatusService) : ICommandHandler<CreateCheckoutCommand, string>
    {
        private readonly IPaymentStatusService _paymentStatusService = paymentStatusService;
        private readonly IPaymentGateway _paymentGateway = paymentGateway;

        public async Task<string> Handle(CreateCheckoutCommand request, CancellationToken ct)
        {
            var intent = new PaymentIntentDTO
            {
                UserId = request.User.Id,
                PaymentIntentId = Guid.NewGuid().ToString()
            };

            await _paymentStatusService.CreatePaymentIntent(intent);

            return await _paymentGateway.CreateCheckoutSessionAsync(
                request.User.Id,
                request.SuccessUrl + $"?paymentId={intent.PaymentIntentId}",
                request.CancelUrl, ct
            );
        }
    }
}