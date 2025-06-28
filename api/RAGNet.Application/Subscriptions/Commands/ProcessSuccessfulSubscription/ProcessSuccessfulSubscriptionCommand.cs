using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Application.Subscriptions.Commands.ProcessSuccessfulSubscription
{
    public class ProcessSuccessfulSubscriptionCommand : BaseCommand<Unit>
    {
        public string UserId { get; } = string.Empty;
        public string PaymentId { get; } = string.Empty;
        public string SubscriptionId { get; } = string.Empty;
        public PlanType PlanType { get; }

        public ProcessSuccessfulSubscriptionCommand(PaymentIntentDTO dto)
        {
            UserId = dto.UserId;
            PaymentId = dto.PaymentIntentId;
            SubscriptionId = dto.SubscriptionId;
            if (!Enum.TryParse<PlanType>(dto.PlanType, ignoreCase: true, out var parsedPlanType))
            {
                throw new ArgumentException($"Invalid plan type: {dto.PlanType}");
            }

            PlanType = parsedPlanType;
        }
    }
}