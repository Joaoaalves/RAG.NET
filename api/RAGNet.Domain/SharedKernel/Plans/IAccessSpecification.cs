using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans
{
    public interface IAccessSpecification<T>
    {
        bool IsSatisfiedBy(PlanType plan, T strategy);
    }
}