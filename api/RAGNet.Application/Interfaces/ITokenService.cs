using System.Security.Claims;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool IsTokenExpired(string token);
    }
}