using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Users.Subscriptions;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Subscriptions
{
    public class SubscriptionRepository(
        ApplicationDbContext context
    ) : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Subscription?> GetByUserIdAsync(string userId)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public Task UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            return Task.CompletedTask;
        }
    }
}