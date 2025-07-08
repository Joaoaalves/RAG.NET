using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.VectorStorages
{
    public class VectorStorageId : TypedIdValueBase
    {
        public VectorStorageId(Guid value) : base(value) { }
        public VectorStorageId() : base(Guid.NewGuid()) { }
    }
}