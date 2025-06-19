using Microsoft.AspNetCore.Identity;

using RAGNET.Application.Configuration.Queries;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Users.GetUserDetails
{
    public class GetUserDetailsQueryHandler(
        UserManager<User> userManager
    ) : IQueryHandler<GetUserDetailsQuery, UserDetailsDTO>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<UserDetailsDTO> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.Principal) ?? throw new Exception("Invalid user!");

            return user.ToAccountInfoDTOFromUser();
        }
    }
}