using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommand(
        string firstName,
        string lastName,
        string email,
        string password
    ) : BaseCommand<(string userId, IEnumerable<string> Errors)>
    {
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}