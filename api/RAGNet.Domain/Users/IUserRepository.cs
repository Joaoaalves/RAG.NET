using Microsoft.AspNetCore.Identity;

namespace RAGNET.Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(string userId);
        Task<User?> GetByCustomerIdAsync(string customerId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> CheckPassowrd(User user, string password);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(string userId);
    }
}

