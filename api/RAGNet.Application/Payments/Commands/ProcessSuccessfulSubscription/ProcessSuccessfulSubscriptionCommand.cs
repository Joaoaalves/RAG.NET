using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Payments.DTOs;

namespace RAGNET.Application.Payments.Commands.ProcessSuccessfulSubscription
{
    public class ProcessSuccessfulSubscriptionCommand(
        PaymentIntentDTO intent
    ) : BaseCommand<Unit>
    {
        public PaymentIntentDTO Intent { get; } = intent;
    }
}