using Microsoft.AspNetCore.Identity;
using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.SharedKernel.Users;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Users.RegisterUser
{
    public class RegisterUserCommandHandler(
        UserManager<User> userManager
    ) : ICommandHandler<RegisterUserCommand, (bool sucess, IEnumerable<string> Errors)>
    {
        private readonly UserManager<User> _userManager = userManager;
        public async Task<(bool sucess, IEnumerable<string> Errors)> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var firstName = new Name(request.FirstName);
            var lastName = new Name(request.LastName);
            var email = new Email(request.Email);
            var userName = new UserName(request.Email);

            var user = User.Create(firstName, lastName, userName, email);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            return (true, []);
        }
    }
}