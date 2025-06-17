using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RAGNET.Application.DTOs.Account;
using RAGNET.Application.Mappers;
using RAGNET.Domain.Users;

namespace RAGNET.Application.UseCases.Account
{
    public interface IGetUserInfo
    {
        Task<AccountInfoDTO?> ExecuteAsync(ClaimsPrincipal principal);
    }

    public class GetUserInfo(UserManager<User> userManager) : IGetUserInfo
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<AccountInfoDTO?> ExecuteAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);

            return user?.ToAccountInfoDTOFromUser();
        }
    }
}