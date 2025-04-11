using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.ContentFilter;
using RAGNET.Application.DTOs.Embedder;
using RAGNET.Application.DTOs.QueryEnhancer;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.Workflow
{
    public class WorkflowDetailsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CollectionId { get; set; }
        public int DocumentsCount { get; set; }
        [JsonConverter(typeof(ChunkerStrategyConverter))]
        public ChunkerStrategy? Strategy { get; set; }
        public ChunkerSettingsDTO? Settings { get; set; } = null!;
        public string ApiKey { get; set; } = string.Empty;
        public EmbeddingProviderConfigDTO? EmbeddingProvider { get; set; } = null!;
        public ConversationProviderConfigDTO? ConversationProvider { get; set; } = null!;
        public ICollection<QueryEnhancerDTO> QueryEnhancers { get; set; } = new List<QueryEnhancerDTO>();
        public FilterDTO? Filter { get; set; }
    }
}