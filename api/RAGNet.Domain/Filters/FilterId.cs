using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Filters
{
    public class FilterId : TypedIdValueBase
    {
        public FilterId(Guid value) : base(value)
        { }
        public FilterId() : base(Guid.NewGuid())
        { }
    }
}