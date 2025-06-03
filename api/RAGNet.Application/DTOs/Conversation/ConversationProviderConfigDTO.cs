using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.DTOs.Conversation
{
    public class ConversationProviderConfigDTO
    {
        [JsonConverter(typeof(ConversationServiceConverter))]
        public ConversationProviderEnum Provider { get; set; }
        public string Model { get; set; } = String.Empty;
    }
}