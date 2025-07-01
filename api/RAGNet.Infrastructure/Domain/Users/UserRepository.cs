using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RAGNET.Domain.Users;
using RAGNET.Infrastructure.Database;

namespace RAGNET.Infrastructure.Domain.Users
{
    public class UserRepository(
        UserManager<User> userManager,
        ApplicationDbContext applicationDbContext
    ) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ApplicationDbContext _dbContext = applicationDbContext;
        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            return await _userManager.DeleteAsync(user);
        }

        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _dbContext.Users
                .Include(u => u.Subscription)
                .Include(u => u.TokenWallet)
                .Include(u => u.Workflows)
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.Email == userId);
        }

        public async Task<User?> GetByCustomerIdAsync(string customerId)
        {
            return await _dbContext.Users
                .Include(u => u.Subscription)
                .Include(u => u.TokenWallet)
                .Include(u => u.Workflows)
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.CustomerId == customerId);
        }


        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> CheckPassowrd(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}