using RAGNET.Application.Configuration.Commands;

namespace RAGNET.Application.Users.RegisterUser
{
    public class RegisterUserCommand(
        string firstName,
        string lastName,
        string email,
        string password
    ) : ICommand<(bool sucess, IEnumerable<string> Errors)>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}