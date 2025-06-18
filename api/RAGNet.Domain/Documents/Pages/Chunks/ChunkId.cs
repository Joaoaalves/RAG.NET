using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Pages.Chunks
{
    public class ChunkId : TypedIdValueBase
    {
        public ChunkId(Guid value) : base(value) { }
        public ChunkId() : base(Guid.NewGuid()) { }
    }
}