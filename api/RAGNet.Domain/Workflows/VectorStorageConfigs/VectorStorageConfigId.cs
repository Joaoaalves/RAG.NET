using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Workflows.VectorStorageConfigs
{
    public class VectorStorageConfigId : TypedIdValueBase
    {
        public VectorStorageConfigId(Guid value) : base(value) { }
        public VectorStorageConfigId() : base(Guid.NewGuid()) { }
    }
}