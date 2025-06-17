using Microsoft.EntityFrameworkCore;

using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.ProviderApiKeys
{
    public class ProviderApiKeyRepository(ApplicationDbContext context) : IProviderApiKeyRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ProviderApiKey> AddAsync(ProviderApiKey entity)
        {

            var exists = await _context.ProviderApiKeys.FirstOrDefaultAsync(
                p => p.UserId == entity.UserId && p.Provider == entity.Provider
            );

            if (exists != null)
                throw new Exception("You've already set an API Key on this Provider.");

            await _context.ProviderApiKeys.AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(ProviderApiKey entity, string? userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            if (entity.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this API key.");
            }

            _context.ProviderApiKeys.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(SupportedProvider provider, string userId)
        {
            return await _context.ProviderApiKeys
                .AnyAsync(p => p.Provider.Id == provider && p.UserId == userId);
        }

        public Task<ProviderApiKey?> GetByIdAsync(Guid id, string? userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            return _context.ProviderApiKeys
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        }

        public Task<ProviderApiKey?> GetByUserIdAndProviderAsync(string userId, SupportedProvider provider)
        {
            return _context.ProviderApiKeys
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Provider.Id == provider);
        }

        public Task<IEnumerable<ProviderApiKey>> GetByUserIdAsync(string userId)
        {
            return _context.ProviderApiKeys
                .Where(p => p.UserId == userId)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<ProviderApiKey>)t.Result);
        }

        public Task UpdateAsync(ProviderApiKey entity, string? userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            if (entity.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this API key.");
            }

            _context.ProviderApiKeys.Update(entity);
            return Task.CompletedTask;
        }
    }
}