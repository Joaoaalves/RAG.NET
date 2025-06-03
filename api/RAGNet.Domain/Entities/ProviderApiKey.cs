using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Domain.Entities
{
    public class ProviderApiKey
    {
        public Guid Id { get; set; }
        public Provider Provider { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
        public User User { get; set; } = null!;
    }
}