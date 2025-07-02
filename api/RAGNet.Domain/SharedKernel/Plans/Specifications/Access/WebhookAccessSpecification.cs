using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans.Specifications.Access
{
    public enum WebhookAccess
    {
        Required
    }
    public class WebhookAccessSpecification : IAccessSpecification<WebhookAccess>
    {
        public bool IsSatisfiedBy(PlanType plan, WebhookAccess _)
        {
            return plan == PlanType.Ascend;
        }
    }
}