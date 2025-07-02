using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RAGNET.Application.Configuration.Queries;
using RAGNET.Application.Users.Mappers;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQueryHandler(
        UserManager<User> userManager
    ) : IQueryHandler<GetUserDetailsQuery, UserDetailsDTO>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<UserDetailsDTO> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(u => u.TokenWallet)
                .Include(u => u.Subscription)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(request.Principal), cancellationToken: cancellationToken) ?? throw new Exception("Invalid user!");
            return user.ToUserDetailsDTO();
        }
    }
}