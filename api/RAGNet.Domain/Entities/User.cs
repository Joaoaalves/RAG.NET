using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RAGNET.Domain.Entities
{
    [Table("user")]
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Workflow> Workflows { get; set; } = [];
        public ICollection<ProviderApiKey> ApiKeys { get; set; } = [];
    }
}