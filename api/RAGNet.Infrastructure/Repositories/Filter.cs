using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class FilterRepository(ApplicationDbContext context) : Repository<Filter>(context), IFilterRepository
    {
        public async Task<IEnumerable<Filter>> GetWithMetaAsync(Guid id)
        {
            return await _context.Filters.Include(f => f.Metas).Where(f => f.Id == id).ToListAsync();
        }
    }
}