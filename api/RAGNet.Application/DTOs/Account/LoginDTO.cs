using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.DTOs.Account
{
    public class LoginDTO
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}