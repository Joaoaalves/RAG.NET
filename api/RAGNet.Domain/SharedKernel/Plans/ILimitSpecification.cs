using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans
{
    public interface ILimitSpecification<T>
    {
        int GetLimit(PlanType plan);
        bool IsWithinLimit(PlanType plan, IReadOnlyCollection<T> existingItems);
    }
}