using System.ComponentModel.DataAnnotations;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.ProviderApiKeys.Commands.CreateProviderApiKey
{
    public class CreateProviderApiKeyRequest
    {
        [Required]
        public string ApiKey { get; set; } = String.Empty;
        [Required]
        public SupportedProvider Provider { get; set; }
    }
}