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

        public async Task<(List<TokenTransaction> Transactions, int TotalCount)> GetPagedTransactionsAsync(
            string userId,
            int page,
            int pageSize,
            DateTime? start = null,
            DateTime? end = null
        )
        {
            var query = _context.TokenTransactions
                .Where(t => t.UserId == userId);

            if (start.HasValue)
                query = query.Where(t => t.TimeStamp >= start.Value);

            if (end.HasValue)
                query = query.Where(t => t.TimeStamp <= end.Value);

            var totalCount = await query.CountAsync();

            var transactions = await query
                .OrderByDescending(t => t.TimeStamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (transactions, totalCount);
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
            var query = await _context.TokenTransactions
                .Where(t => t.TimeStamp >= start &&
                            t.TimeStamp <= end &&
                            t.UserId == userId)
                .GroupBy(t => t.TimeStamp.Date)
                .Select(g => new TransactionDailyAggregation
                {
                    Date = g.Key,
                    FreeTokensConsumed = (double)g.Where(t => t.Source == TokenSource.Free)
                                          .Sum(t => EF.Property<long>(t, "CostValue") / 1000.0),
                    PaidTokensConsumed = (double)g.Where(t => t.Source == TokenSource.Paid)
                                          .Sum(t => EF.Property<long>(t, "CostValue") / 1000.0),
                })
                .ToListAsync();

            return query;
        }

    }
}