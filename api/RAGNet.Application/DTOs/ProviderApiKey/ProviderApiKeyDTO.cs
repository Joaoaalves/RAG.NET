using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.DTOs.ProviderApiKey
{
    public class ProviderApiKeyDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string ApiKey { get; set; } = String.Empty;
        public SupportedProvider ProviderId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Pattern { get; set; } = String.Empty;
        public string Prefix { get; set; } = String.Empty;
        public string Url { get; set; } = String.Empty;
    }
}