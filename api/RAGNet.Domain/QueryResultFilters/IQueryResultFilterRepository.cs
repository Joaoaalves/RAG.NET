namespace RAGNET.Domain.QueryResultFilters
{
    public interface IQueryResultFilterRepository
    {
        Task<QueryResultFilter> AddAsync(QueryResultFilter entity);
        Task<QueryResultFilter?> GetByIdAsync(QueryResultFilterId id, string? userId);
        Task UpdateAsync(QueryResultFilter entity, string? userId);
        Task DeleteAsync(QueryResultFilter entity, string? userId);
    }
}