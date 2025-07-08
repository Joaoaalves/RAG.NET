using RAGNET.Domain.Chunkers;

using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;
using RAGNET.Application.Chunkers.DTOs;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{
    public class WorkflowCreationRequest
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public ChunkerStrategy Strategy { get; set; }
        public ChunkerSettingsDTO Settings { get; set; } = null!;
        public EmbeddingProviderConfigDTO EmbeddingProvider { get; set; } = null!;
        public ConversationProviderConfigDTO ConversationProvider { get; set; } = null!;
        public Guid VectorStorageId { get; set; }
    }
}