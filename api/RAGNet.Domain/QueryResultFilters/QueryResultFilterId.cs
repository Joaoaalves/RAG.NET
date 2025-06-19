using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.QueryResultFilters
{
    public class QueryResultFilterId : TypedIdValueBase
    {
        public QueryResultFilterId(Guid value) : base(value)
        { }
        public QueryResultFilterId() : base(Guid.NewGuid())
        { }
    }
}