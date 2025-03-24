using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{
    public class EmbeddingProvider
    {
        private string Id { get; set; } = String.Empty;
        private EmbeddingProviderTypeEnum Provider { get; set; }
        private string ApiKey { get; set; } = String.Empty;
    }
}