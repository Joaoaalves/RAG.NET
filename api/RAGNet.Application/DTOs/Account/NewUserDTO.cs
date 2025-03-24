using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.DTOs.Account
{
    public class NewUserDTO
    {
        [Required]
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}