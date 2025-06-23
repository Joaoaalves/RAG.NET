using RAGNET.Application.Users.Queries.GetUserDetails;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Users.Mappers
{
    public static class UserMapper
    {
        public static UserDetailsDTO ToUserDetailsDTO(this User user)
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