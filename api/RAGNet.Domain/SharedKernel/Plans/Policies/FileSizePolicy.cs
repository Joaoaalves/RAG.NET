using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Plans.Policies
{
    public interface IFileSizePolicy
    {
        bool IsSatisfiedBy(PlanType plan, long fileSizeInBytes);
    }
    public class FileSizePolicy : IFileSizePolicy
    {
        public bool IsSatisfiedBy(PlanType plan, long fileSizeInBytes)
        {
            return plan switch
            {
                PlanType.Core => fileSizeInBytes <= 10 * 1024 * 1024,        // 10 MB
                PlanType.Enhanced => fileSizeInBytes <= 50 * 1024 * 1024,   // 50 MB
                PlanType.Ascend => fileSizeInBytes <= 500 * 1024 * 1024,     // 500 MB
                _ => false
            };
        }
    }
}