using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;
using RAGNET.Domain.SharedKernel.ApiKeys;

namespace RAGNET.Domain.Entities
{
    public class ProviderApiKey
    {
        public Guid Id { get; set; }
        public SupportedProvider Provider { get; set; }
        public ApiKey ApiKey { get; set; } = null!;
        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
        public User User { get; set; } = null!;
    }
}