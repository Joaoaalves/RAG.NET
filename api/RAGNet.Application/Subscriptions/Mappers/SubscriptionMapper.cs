using RAGNET.Application.Subscriptions.DTOs;
using RAGNET.Domain.Users.Subscriptions;

namespace RAGNET.Application.Subscriptions.Mappers
{
    public static class SubscriptionMapper
    {
        public static SubscriptionDTO ToDTO(this Subscription subscription)
        {
            return new SubscriptionDTO
            {
                PlanType = subscription.Plan.Value,
                SubscribedAt = subscription.SubscribedAt,
                ExpiresAt = subscription.ExpiresAt
            };
        }
    }
}