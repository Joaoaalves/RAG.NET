using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class ProviderApiKeyRepository(ApplicationDbContext context) : Repository<ProviderApiKey>(context), IProviderApiKeyRepository
    {
        public Task<bool> ExistsAsync(SupportedProvider provider, string userId)
        {
            return _context.ProviderApiKeys
                .AnyAsync(u => u.Provider == provider && u.UserId == userId);
        }

        public async Task<ProviderApiKey?> GetByUserIdAndProviderAsync(string userId, SupportedProvider provider)
        {
            return await _context.ProviderApiKeys
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Provider == provider);
        }

        public async Task<IEnumerable<ProviderApiKey>> GetByUserIdAsync(string userId)
        {
            return await _context.ProviderApiKeys
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }
    }
}