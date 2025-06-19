using RAGNET.Application.Users.GetUserDetails;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Users
{
    public static class UserMapper
    {
        public static UserDetailsDTO ToAccountInfoDTOFromUser(this User user)
        {
            return new UserDetailsDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? ""
            };
        }
    }
}