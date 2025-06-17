namespace RAGNET.Domain.Chunkers
{
    public interface IChunkerRepository
    {
        Task<IEnumerable<Chunker>> GetWithMetaAsync(Guid id);
        Task<Chunker> AddAsync(Chunker chunker);
        Task UpdateAsync(Chunker chunker, Guid workflowId, string userId);
        Task DeleteAsync(Chunker chunker, Guid workflowId, string userId);
        Task<Chunker?> GetByIdAsync(Guid id, Guid workflowId, string userId);
    }
}