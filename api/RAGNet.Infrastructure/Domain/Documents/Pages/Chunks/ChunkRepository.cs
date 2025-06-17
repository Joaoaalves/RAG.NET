using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Documents.Pages.Chunks
{
    public class ChunkRepository(ApplicationDbContext context) : IChunkRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Chunk> AddAsync(Chunk chunk)
        {
            await _context.Chunks.AddAsync(chunk);
            return chunk;
        }

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