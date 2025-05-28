using System.Text.Json.Serialization;
using RAGNET.Application.Converters;
using RAGNET.Application.DTOs.CallbackUrl;
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
        public ChunkerSettingsDTO? Settings { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public EmbeddingProviderConfigDTO? EmbeddingProvider { get; set; }
        public ConversationProviderConfigDTO? ConversationProvider { get; set; }
        public ICollection<QueryEnhancerDTO> QueryEnhancers { get; set; } = [];
        public ICollection<CallbackUrlDTO> CallbackUrls { get; set; } = [];
        public FilterDTO? Filter { get; set; }
    }
}