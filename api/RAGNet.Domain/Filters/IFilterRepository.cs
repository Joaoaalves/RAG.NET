namespace RAGNET.Domain.Filters
{
    public interface IFilterRepository
    {
        Task<Filter> AddAsync(Filter entity);
        Task<Filter?> GetByIdAsync(Guid id, string? userId);
        Task UpdateAsync(Filter entity, string? userId);
        Task DeleteAsync(Filter entity, string? userId);
    }
}