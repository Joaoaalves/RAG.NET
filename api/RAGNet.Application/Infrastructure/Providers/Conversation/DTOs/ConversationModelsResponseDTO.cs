using System.Text.Json.Serialization;
using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers.Conversation.DTOs
{
    public class ConversationProviderDTO
    {
        public SupportedProvider ProviderId { get; set; }
        [JsonConverter(typeof(SupportedProviderConverter))]
        public SupportedProvider ProviderName { get; set; }
        public List<ConversationModel> Models { get; set; } = [];

        internal ConversationProviderConfig ToConversationProviderConfig()
        {
            throw new NotImplementedException();
        }
    }


    public class ConversationModelsResponseDTO
    {
        public List<ConversationProviderDTO> Providers { get; set; } = [];
    }
}