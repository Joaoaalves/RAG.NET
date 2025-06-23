using System.Text.Json.Serialization;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers.Conversation
{
    public class ConversationProviderConfigDTO
    {
        public ConversationProviderEnum ProviderId { get; set; }
        [JsonConverter(typeof(ConversationServiceConverter))]
        public ConversationProviderEnum ProviderName { get; set; }
        public string Model { get; set; } = String.Empty;
    }
}