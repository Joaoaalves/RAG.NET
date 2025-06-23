using System.Text.Json.Serialization;
using RAGNET.Application.Infrastructure.Providers.Embedding.Converters;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers.Embedding.DTOs
{
    public class EmbeddingProviderConfigDTO
    {
        public EmbeddingProviderEnum ProviderId { get; set; }

        [JsonConverter(typeof(EmbeddingServiceConverter))]
        public EmbeddingProviderEnum ProviderName { get; set; }
        public string Model { get; set; } = String.Empty;
        [JsonIgnore]
        public int VectorSize { get; set; }
    }
}