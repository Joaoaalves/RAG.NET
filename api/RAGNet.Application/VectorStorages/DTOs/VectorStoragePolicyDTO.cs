using RAGNET.Domain.SharedKernel.VectorStorages;

namespace RAGNET.Application.VectorStorages.DTOs
{
    public class VectorStoragePolicyDTO
    {
        public Guid Id { get; set; }
        public VectorStorageProvider ProviderId { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string Name { get; set; } = null!;
        public string Pattern { get; set; } = String.Empty;
        public string Prefix { get; set; } = String.Empty;
        public string Url { get; set; } = String.Empty;
        public string Schema { get; set; } = string.Empty;
    }
}