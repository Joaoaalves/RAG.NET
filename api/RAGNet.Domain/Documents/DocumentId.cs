using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents
{
    public class DocumentId : TypedIdValueBase
    {
        public DocumentId(Guid value) : base(value)
        {
        }

        public DocumentId() : base(Guid.NewGuid())
        {
        }
    }
}