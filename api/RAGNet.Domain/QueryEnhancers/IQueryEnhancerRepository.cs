namespace RAGNET.Domain.QueryEnhancers
{
    public interface IQueryEnhancerRepository
    {
        Task<QueryEnhancer?> GetByIdAsync(Guid id, string userId);
        Task<QueryEnhancer> AddAsync(QueryEnhancer queryEnhancer);
        Task UpdateAsync(QueryEnhancer queryEnhancer);
        Task DeleteAsync(QueryEnhancer queryEnhancer);
    }
}