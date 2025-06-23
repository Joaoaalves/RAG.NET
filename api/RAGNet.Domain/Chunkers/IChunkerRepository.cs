using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.Chunkers
{
    public interface IChunkerRepository
    {
        Task<IEnumerable<Chunker>> GetWithMetaAsync(ChunkerId id);
        Task<Chunker> AddAsync(Chunker chunker);
        Task UpdateAsync(Chunker chunker, WorkflowId workflowId, string userId);
        Task DeleteAsync(Chunker chunker, WorkflowId workflowId, string userId);
        Task<Chunker?> GetByIdAsync(ChunkerId id, WorkflowId workflowId, string userId);
    }
}