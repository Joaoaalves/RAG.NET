using System.Security.Claims;
using RAGNET.Application.Configuration.Queries;

namespace RAGNET.Application.Users.GetUserDetails
{
    public class GetUserDetailsQuery(
        ClaimsPrincipal claimsPrincipal
    ) : IQuery<UserDetailsDTO>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public ClaimsPrincipal Principal { get; } = claimsPrincipal;
    }
}