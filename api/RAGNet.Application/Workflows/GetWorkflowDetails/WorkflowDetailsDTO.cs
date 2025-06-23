using System.Text.Json.Serialization;

using RAGNET.Domain.Chunkers;

using RAGNET.Application.Workflows.CallbackUrls;
using RAGNET.Application.QueryEnhancers;
using RAGNET.Application.QueryResultFilters;
using RAGNET.Application.Chunkers;
using RAGNET.Application.Providers.Conversation;
using RAGNET.Application.Providers.Embedding;

namespace RAGNET.Application.Workflows.GetWorkflowDetails
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