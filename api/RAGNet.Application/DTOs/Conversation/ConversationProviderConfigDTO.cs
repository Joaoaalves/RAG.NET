using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.Embedder
{
    public class ConversationProviderConfigDTO
    {
        [JsonConverter(typeof(ConversationServiceConverter))]
        public ConversationProviderEnum Provider { get; set; }
        public string Model { get; set; } = String.Empty;
    }
}