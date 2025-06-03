using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.DTOs.ProviderApiKey
{
    public class CreateProviderApiKeyDTO
    {
        public string ApiKey { get; set; } = String.Empty;
        public SupportedProvider Provider { get; set; }
    }
}