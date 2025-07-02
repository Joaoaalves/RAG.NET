using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace tests.RAGNet.Domain.Tests.Chunkers
{
    public static class DummyChunker
    {
        public static Chunker CreateDummy(
            ChunkerStrategy? strategyType = null,
            WorkflowId? workflowId = null,
            string? userId = null,
            IEnumerable<Meta>? metas = null,
            ChunkerId? chunkerId = null)
        {
            return Chunker.Create(
                strategyType ?? ChunkerStrategy.PARAGRAPH,
                workflowId ?? new WorkflowId(Guid.NewGuid()),
                userId ?? "dummy-user",
                metas ?? [new("key1", "value1"), new("key2", "value2")],
                chunkerId);
        }
    }
}