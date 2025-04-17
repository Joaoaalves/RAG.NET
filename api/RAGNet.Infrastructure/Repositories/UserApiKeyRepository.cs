using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class UserApiKeyRepository(ApplicationDbContext context) : Repository<UserApiKey>(context), IUserApiKeyRepository
    {
        public Task<bool> ExistsAsync(SupportedProvider provider, string userId)
        {
            return _context.UserApiKeys
                .AnyAsync(u => u.Provider == provider && u.UserId == userId);
        }

        public async Task<UserApiKey?> GetByUserIdAndProviderAsync(string userId, SupportedProvider provider)
        {
            return await _context.UserApiKeys
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Provider == provider);
        }

        public async Task<IEnumerable<UserApiKey>> GetByUserIdAsync(string userId)
        {
            return await _context.UserApiKeys
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }
    }
}