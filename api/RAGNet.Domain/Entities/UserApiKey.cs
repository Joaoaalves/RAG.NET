using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{
    public class UserApiKey
    {
        public Guid Id { get; set; }
        public SupportedProvider Provider { get; set; }
        public string ApiKey { get; set; } = String.Empty;
        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
        public User User { get; set; } = null!;
    }
}