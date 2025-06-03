using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.DTOs.Conversation
{
    public class ConversationProviderDTO
    {
        public SupportedProvider ProviderId { get; set; }
        [JsonConverter(typeof(SupportedProviderConverter))]
        public SupportedProvider ProviderName { get; set; }
        public List<ConversationModel> Models { get; set; } = [];
    }


    public class ConversationModelsResponseDTO
    {
        public List<ConversationProviderDTO> Providers { get; set; } = [];
    }
}