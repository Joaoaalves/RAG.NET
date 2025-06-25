using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.SharedKernel.Tokens;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.TokenWallets.TokenTransactions;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.TokenWallets
{
    public class TokenWalletRepository(ApplicationDbContext context) : ITokenWalletRepository
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<TokenWallet?> GetByUserIdAsync(string userId)
        {
            return await _context.TokenWallets
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }
        public Task UpdateAsync(TokenWallet wallet)
        {
            _context.TokenWallets.Update(wallet);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TokenWallet wallet)
        {
            _context.TokenWallets.Remove(wallet);
            return Task.CompletedTask;
        }
        public async Task<List<TransactionDailyAggregation>> GetTransactionDailyAggregations(DateTime start, DateTime end, string userId)
        {
            // This should be optimized; 
            // Loading all data into memory can lead to high memory consumption.

            // This approach was necessary because LINQ was unable to translate 
            // the query into SQL correctly.
            var freeTokens = await _context.TokenTransactions
                .Where(
                    t => t.TimeStamp >= start &&
                                        t.TimeStamp <= end &&
                                        t.UserId == userId &&
                                        t.Source == TokenSource.Free
                )
                .Select(
                    t => new
                    {
                        t.TimeStamp,
                        Cost = t.Cost.Value
                    })
                .ToListAsync();

            var paidTokens = await _context.TokenTransactions
                .Where(
                    t => t.TimeStamp >= start &&
                         t.TimeStamp <= end &&
                         t.UserId == userId &&
                         t.Source == TokenSource.Paid
                )
                .Select(
                    t => new
                    {
                        t.TimeStamp,
                        Cost = t.Cost.Value
                    })
                .ToListAsync();

            // Combine the data, group by date, and calculate sums in-memory
            var result = freeTokens
                .Concat(paidTokens)
                .GroupBy(x => x.TimeStamp.Date)
                .Select(g => new TransactionDailyAggregation
                {
                    Date = g.Key,
                    FreeTokensConsumed = g.Where(x => x.Cost > 0).Sum(x => x.Cost),
                    PaidTokensConsumed = g.Where(x => x.Cost < 0).Sum(x => x.Cost)
                })
                .ToList();

            return result;
        }

    }
}