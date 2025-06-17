namespace RAGNET.Domain.Rankers
{
    public interface IRankerRepository
    {
        Task<Ranker?> GetByIdAsync(Guid id, string userId);
        Task<Ranker?> AddAsync(Ranker ranker);
        Task UpdateAsync(Ranker ranker);
        Task DeleteAsync(Ranker ranker);
    }
}