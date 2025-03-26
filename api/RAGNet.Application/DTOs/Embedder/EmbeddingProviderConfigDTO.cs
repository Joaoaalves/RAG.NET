using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.Embedder
{
    public class EmbeddingProviderConfigDTO
    {
        [JsonConverter(typeof(EmbeddingServiceConverter))]
        public EmbeddingProviderEnum Provider { get; set; }
        public string ApiKey { get; set; } = String.Empty;
        public int VectorSize { get; set; }
    }
}