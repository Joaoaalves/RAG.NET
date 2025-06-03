using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.DTOs.Embedding
{
    public class EmbeddingProviderConfigDTO
    {
        public EmbeddingProviderEnum ProviderId { get; set; }

        [JsonConverter(typeof(EmbeddingServiceConverter))]
        public EmbeddingProviderEnum ProviderName { get; set; }
        public string Model { get; set; } = String.Empty;
        [JsonIgnore]
        public int VectorSize { get; set; } = 0;
    }
}