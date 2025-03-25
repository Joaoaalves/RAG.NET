using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class RankerRepository(ApplicationDbContext context) : Repository<Ranker>(context), IRankerRepository
    {
        public async Task<IEnumerable<Ranker>> GetWithMetaAsync(Guid id)
        {
            return await _context.Rankers
                .Include(r => r.Metas)
                .Where(r => r.Id == id)
                .ToListAsync();
        }
    }
}