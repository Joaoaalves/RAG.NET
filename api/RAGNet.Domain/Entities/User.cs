using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RAGNET.Domain.Entities
{
    [Table("user")]
    public class User : IdentityUser
    {
        public string Name { get; private set; } = string.Empty;

        public ICollection<Workflow> Workflows { get; set; } = [];
    }
}