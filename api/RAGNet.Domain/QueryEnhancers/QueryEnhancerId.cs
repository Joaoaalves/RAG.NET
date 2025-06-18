using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.QueryEnhancers
{
    public class QueryEnhancerId : TypedIdValueBase
    {
        public QueryEnhancerId(Guid value) : base(value)
        {
        }

        public QueryEnhancerId() : base(Guid.NewGuid())
        {
        }
    }
}