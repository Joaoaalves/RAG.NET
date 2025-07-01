using RAGNET.Domain.Workflows;

using RAGNET.Application.Configuration.Commands;
using RAGNET.Domain.Chunkers;
using RAGNET.Application.Chunkers.DTOs;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{
    public class CreateWorkflowCommand(WorkflowCreationRequest request) : UserAwareCommand<WorkflowId>
    {
        public string Name { get; set; } = request.Name;
        public string Description { get; set; } = request.Description;
        public ChunkerStrategy Strategy { get; set; } = request.Strategy;
        public ChunkerSettingsDTO Settings { get; set; } = request.Settings;
        public EmbeddingProviderConfigDTO EmbeddingProvider { get; set; } = request.EmbeddingProvider;
        public ConversationProviderConfigDTO ConversationProvider { get; set; } = request.ConversationProvider;
    }
}