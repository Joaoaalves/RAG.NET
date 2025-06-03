using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.Conversation;
using RAGNET.Application.DTOs.Embedding;
using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.Workflow
{
    public class WorkflowCreationDTO
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public ChunkerStrategy Strategy { get; set; }
        public ChunkerSettingsDTO Settings { get; set; } = null!;
        public EmbeddingProviderConfigDTO EmbeddingProvider { get; set; } = null!;
        public ConversationProviderConfigDTO ConversationProvider { get; set; } = null!;
    }
}