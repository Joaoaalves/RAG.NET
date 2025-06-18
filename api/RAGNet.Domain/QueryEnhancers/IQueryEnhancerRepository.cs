namespace RAGNET.Domain.QueryEnhancers
{
    public interface IQueryEnhancerRepository
    {
        Task<QueryEnhancer?> GetByIdAsync(QueryEnhancerId id, string userId);
        Task<QueryEnhancer> AddAsync(QueryEnhancer queryEnhancer);
        Task UpdateAsync(QueryEnhancer queryEnhancer);
        Task DeleteAsync(QueryEnhancer queryEnhancer);
    }
}