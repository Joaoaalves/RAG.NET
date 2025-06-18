using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Filters;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Filters
{
    public class FilterRepository(ApplicationDbContext context) : IFilterRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Filter> AddAsync(Filter entity)
        {
            await _context.Filters.AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(Filter entity, string? userId)
        {
            if (userId != null && entity.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this filter.");
            }

            _context.Filters.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<Filter?> GetByIdAsync(FilterId id, string? userId)
        {
            return await _context.Filters
                .FirstOrDefaultAsync(f => f.Id == id && (userId == null || f.UserId == userId));
        }

        public Task UpdateAsync(Filter entity, string? userId)
        {
            _context.Filters.Update(entity);
            return Task.CompletedTask;
        }
    }
}