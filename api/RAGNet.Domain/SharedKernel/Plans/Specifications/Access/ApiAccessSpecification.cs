using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans.Specifications.Access
{
    public enum ApiAccess
    {
        Required
    }
    public class ApiAccessSpecification : IAccessSpecification<ApiAccess>
    {
        public bool IsSatisfiedBy(PlanType plan, ApiAccess _)
        {
            return plan != PlanType.Core;
        }
    }
}