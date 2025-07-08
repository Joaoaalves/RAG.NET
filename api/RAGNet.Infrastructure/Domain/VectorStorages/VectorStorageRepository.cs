using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.SharedKernel.VectorStorages;
using RAGNET.Domain.VectorStorages;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.VectorStorages
{
    public class VectorStorageRepository(ApplicationDbContext context) : IVectorStorageRepository
    {

        private readonly ApplicationDbContext _context = context;
        public async Task<VectorStorage> AddAsync(VectorStorage entity)
        {

            var exists = await _context.VectorStorages.FirstOrDefaultAsync(
                p => p.UserId == entity.UserId && p.Provider == entity.Provider
            );

            if (exists != null)
                throw new Exception("You've already set an API Key on this Provider.");

            await _context.VectorStorages.AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(VectorStorage entity, string userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            if (entity.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this API key.");
            }

            _context.VectorStorages.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<VectorStorage?> GetByIdAsync(VectorStorageId id, string userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            return _context.VectorStorages
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        }

        public Task<VectorStorage?> GetByUserIdAndProviderAsync(string userId, VectorStorageProvider provider)
        {
            return _context.VectorStorages
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Provider == provider);
        }

        public Task<IEnumerable<VectorStorage>> GetByUserIdAsync(string userId)
        {
            return _context.VectorStorages
                .Where(p => p.UserId == userId)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<VectorStorage>)t.Result);
        }

        public Task UpdateAsync(VectorStorage entity, string userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            if (entity.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this API key.");
            }

            _context.VectorStorages.Update(entity);
            return Task.CompletedTask;
        }
    }
}