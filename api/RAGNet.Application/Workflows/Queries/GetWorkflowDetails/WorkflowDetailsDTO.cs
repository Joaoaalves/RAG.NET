using System.Text.Json.Serialization;

using RAGNET.Domain.Chunkers;

using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Application.QueryEnhancers.DTOs;
using RAGNET.Application.Chunkers.DTOs;
using RAGNET.Application.Chunkers.DTOs.Converters;
using RAGNET.Application.QueryResultFilters.DTOs;
using RAGNET.Application.Workflows.CallbackUrls.DTOs;

namespace RAGNET.Application.Workflows.Queries.GetWorkflowDetails
{
    public class WorkflowDetailsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
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
        public QueryResultFilterDTO? QueryResultFilter { get; set; }
    }
}