using RAGNET.Domain.Chunkers;

using RAGNET.Application.Chunkers;
using RAGNET.Application.Providers.Embedding;
using RAGNET.Application.Providers.Conversation;

namespace RAGNET.Application.Workflows.CreateWorkflow
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