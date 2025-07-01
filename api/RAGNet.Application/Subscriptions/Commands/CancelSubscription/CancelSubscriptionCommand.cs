using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommand(
        CancelSubscriptionRequest request
    ) : UserAwareCommand<bool>
    {
        public string UserPassword { get; } = request.Password;
    }
}