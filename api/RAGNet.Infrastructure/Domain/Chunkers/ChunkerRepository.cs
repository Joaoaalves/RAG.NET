using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Chunkers
{
    public class ChunkerRepository(ApplicationDbContext _context) : IChunkerRepository
    {
        private readonly ApplicationDbContext _context = _context ?? throw new ArgumentNullException(nameof(_context));

        public async Task<Chunker> AddAsync(Chunker chunker)
        {
            ArgumentNullException.ThrowIfNull(chunker, nameof(chunker));

            await _context.Chunkers.AddAsync(chunker);

            return chunker;
        }

        public Task DeleteAsync(Chunker chunker, Guid workflowId, string userId)
        {
            _context.Chunkers.Remove(chunker);
            return Task.CompletedTask;
        }

        public async Task<Chunker?> GetByIdAsync(Guid id, Guid workflowId, string userId)
        {
            return await _context.Chunkers
                .Include(c => c.Metas)
                .FirstOrDefaultAsync(c => c.Id == id && c.WorkflowId == workflowId && c.UserId == userId);
        }

        public async Task<IEnumerable<Chunker>> GetWithMetaAsync(Guid id)
        {
            return await _context.Chunkers
                .Include(c => c.Metas)
                .Where(c => c.Id == id)
                .ToListAsync();
        }

        public Task UpdateAsync(Chunker chunker, Guid workflowId, string userId)
        {
            ArgumentNullException.ThrowIfNull(chunker, nameof(chunker));

            _context.Chunkers.Update(chunker);
            return Task.CompletedTask;
        }
    }
}