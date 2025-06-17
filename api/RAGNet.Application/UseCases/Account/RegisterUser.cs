using Microsoft.AspNetCore.Identity;
using RAGNET.Application.DTOs.Account;
using RAGNET.Domain.SharedKernel.Users;
using RAGNET.Domain.Users;

namespace RAGNET.Application.UseCases.Account
{
    public interface IRegisterUser
    {
        Task<(bool Success, IEnumerable<string> Errors)> ExecuteAsync(RegisterDTO model);
    }

    public class RegisterUser(UserManager<User> userManager) : IRegisterUser
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<(bool Success, IEnumerable<string> Errors)> ExecuteAsync(RegisterDTO dto)
        {
            var firstName = new Name(dto.FirstName);
            var lastName = new Name(dto.LastName);
            var email = new Email(dto.Email);
            var userName = new UserName(dto.Email);

            var user = User.Create(firstName, lastName, userName, email);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            return (true, []);
        }
    }
}