using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class ChunkerRepository(ApplicationDbContext context) : Repository<Chunker>(context), IChunkerRepository
    {
        public async Task<IEnumerable<Chunker>> GetWithMetaAsync(Guid id)
        {
            return await _context.Chunkers
                .Include(c => c.Metas)
                .Where(c => c.Id == id)
                .ToListAsync();
        }
    }
}