using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Payments.Services;

namespace RAGNET.Application.Payments.Commands.CreateCheckout
{
    public class CreateCheckoutCommandHandler(IPaymentGateway paymentGateway) : ICommandHandler<CreateCheckoutCommand, string>
    {
        private readonly IPaymentGateway _paymentGateway = paymentGateway;

        public Task<string> Handle(CreateCheckoutCommand request, CancellationToken ct)
        {
            return _paymentGateway.CreateCheckoutSessionAsync(
                request.User.Id,
                request.SuccessUrl,
                request.CancelUrl, ct
            );
        }
    }
}