using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Application.Subscriptions.Services;

namespace RAGNET.Application.Subscriptions.Commands.CreateCheckout
{
    public class CreateCheckoutCommandHandler(IPaymentGateway paymentGateway, IPaymentStatusService paymentStatusService) : ICommandHandler<CreateSubscriptionCheckoutCommand, string>
    {
        private readonly IPaymentStatusService _paymentStatusService = paymentStatusService;
        private readonly IPaymentGateway _paymentGateway = paymentGateway;

        public async Task<string> Handle(CreateSubscriptionCheckoutCommand request, CancellationToken ct)
        {
            var intent = new PaymentIntentDTO
            {
                CustomerId = request.User.CustomerId,
                PaymentIntentId = Guid.NewGuid().ToString(),
                PlanType = request.PlanType
            };

            await _paymentStatusService.CreatePaymentIntent(intent);

            return await _paymentGateway.CreateCheckoutSessionAsync(
                request.User.CustomerId,
                request.SuccessUrl + $"?paymentId={intent.PaymentIntentId}",
                request.CancelUrl,
                request.PlanType,
                ct
            );
        }
    }
}