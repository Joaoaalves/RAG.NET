using System.Security.Claims;
using RAGNET.Domain.Users;

namespace web.Identity
{
    public interface ITokenService
    {
        string CreateToken(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool IsTokenExpired(string token);
    }
}