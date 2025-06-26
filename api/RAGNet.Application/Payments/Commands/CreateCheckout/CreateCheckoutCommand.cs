using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Payments.Commands.CreateCheckout
{
    public class CreateCheckoutCommand(
        string successUrl,
        string cancelUrl
    ) : UserAwareCommand<string>
    {
        public string SuccessUrl { get; init; } = successUrl;
        public string CancelUrl { get; init; } = cancelUrl;
    }
}