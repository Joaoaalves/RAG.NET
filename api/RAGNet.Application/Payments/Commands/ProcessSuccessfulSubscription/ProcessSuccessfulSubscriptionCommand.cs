using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Payments.Commands.ProcessSuccessfulSubscription
{
    public class ProcessSuccessfulSubscriptionCommand(
        string userId
    ) : BaseCommand<Unit>
    {
        public string UserId { get; } = userId;
    }
}