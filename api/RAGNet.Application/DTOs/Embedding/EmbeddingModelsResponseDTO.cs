using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.DTOs.Embedding
{
    public class EmbeddingProviderDTO
    {
        public SupportedProvider ProviderId { get; set; }
        [JsonConverter(typeof(SupportedProviderConverter))]
        public SupportedProvider ProviderName { get; set; }
        public List<EmbeddingModel> Models { get; set; } = [];
    }


    public class EmbeddingModelsResponseDTO
    {
        public List<EmbeddingProviderDTO> Providers { get; set; } = [];
    }
}