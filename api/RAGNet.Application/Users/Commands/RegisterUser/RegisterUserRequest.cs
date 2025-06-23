using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.Users.Commands.RegisterUser
{
    public class RegisterUserRequest
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}