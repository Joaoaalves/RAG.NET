using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RAGNET.Domain.Entities;
using RAGNET.Application.Interfaces;

namespace RAGNET.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
            var signInKey = _config["JWT:SignInKey"] ?? throw new Exception("You should provide JWT SignInKey.");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signInKey));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.GivenName, user.UserName ?? ""),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (securityToken == null) return null!;

            var claims = securityToken?.Claims;
            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        public bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null) return true;

            var expirationDate = jwtToken.ValidTo;
            return expirationDate < DateTime.UtcNow;
        }
    }
}
