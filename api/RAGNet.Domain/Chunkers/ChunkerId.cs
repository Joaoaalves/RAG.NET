using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Chunkers
{
    public class ChunkerId : TypedIdValueBase
    {
        public ChunkerId(Guid value) : base(value) { }
        public ChunkerId() : base(Guid.NewGuid()) { }
    }
}