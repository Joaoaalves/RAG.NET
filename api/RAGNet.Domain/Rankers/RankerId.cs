using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Rankers
{
    public class RankerId : TypedIdValueBase
    {
        public RankerId(Guid value) : base(value)
        {
        }
        public RankerId() : base(Guid.NewGuid())
        {
        }
    }
}