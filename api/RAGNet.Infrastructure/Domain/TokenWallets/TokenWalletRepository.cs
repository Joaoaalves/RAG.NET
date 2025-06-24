using RAGNET.Domain.TokenWallets;
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

        public async Task<TokenWallet> AddAsync(TokenWallet wallet)
        {
            await _context.TokenWallets.AddAsync(wallet);
            return wallet;
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
    }
}