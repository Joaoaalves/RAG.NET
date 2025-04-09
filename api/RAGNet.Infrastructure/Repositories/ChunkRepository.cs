using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class ChunkRepository(ApplicationDbContext context) : Repository<Chunk>(context), IChunkRepository
    {
        public async Task<Chunk?> GetByVectorId(string vectorId)
        {
            return await _context.Chunks.FirstOrDefaultAsync(c => c.VectorId == vectorId);
        }

        public async Task<List<Chunk>> GetManyByVectorId(string[] vectorIds)
        {
            return await _context.Chunks
                .Where(c => vectorIds.Contains(c.VectorId))
                .ToListAsync();
        }
    }
}