using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class QueryEnhancerRepository(ApplicationDbContext context) : Repository<QueryEnhancer>(context), IQueryEnhancerRepository
    {
        public async Task<IEnumerable<QueryEnhancer>> GetWithMetaAsync(Guid id)
        {
            return await _context.QueryEnhancers
                .Include(q => q.Metas)
                .Where(q => q.Id == id)
                .ToListAsync();
        }
    }
}
