using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.DTOs.Account
{
    public class AccountInfoDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
    }
}