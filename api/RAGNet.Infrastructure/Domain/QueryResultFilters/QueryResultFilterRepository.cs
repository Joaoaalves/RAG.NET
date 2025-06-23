using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.QueryResultFilters;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.QueryResultFilters
{
    public class QueryResultFilterRepository(ApplicationDbContext context) : IQueryResultFilterRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<QueryResultFilter> AddAsync(QueryResultFilter entity)
        {
            await _context.Filters.AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(QueryResultFilter entity, string? userId)
        {
            if (userId != null && entity.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this filter.");
            }

            _context.Filters.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<QueryResultFilter?> GetByIdAsync(QueryResultFilterId id, string? userId)
        {
            return await _context.Filters
                .FirstOrDefaultAsync(f => f.Id == id && (userId == null || f.UserId == userId));
        }

        public Task UpdateAsync(QueryResultFilter entity, string? userId)
        {
            _context.Filters.Update(entity);
            return Task.CompletedTask;
        }
    }
}