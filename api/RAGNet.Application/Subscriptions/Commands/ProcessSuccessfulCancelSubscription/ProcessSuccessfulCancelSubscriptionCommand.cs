using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.DTOs;

namespace RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulCancelSubscription
{
    public class ProcessSuccessfulCancelSubscriptionCommand(
        CancelSubscriptionIntentDTO cancelIntent
    ) : BaseCommand<bool>
    {
        public string CustomerId { get; } = cancelIntent.CustomerId;
        public string SubscriptionId { get; } = cancelIntent.SubscriptionId;
        public DateTime CancelAt { get; } = cancelIntent.CancelAt;
    }
}