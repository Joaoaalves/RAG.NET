using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Pages
{
    public class PageId : TypedIdValueBase
    {
        public PageId(Guid value) : base(value) { }

        public PageId() : base(Guid.NewGuid()) { }
    }
}