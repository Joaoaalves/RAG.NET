using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Application.Workflows.Queries.GetWorkflowDetails;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.Commands.UpdateWorkflow
{
    public class UpdateWorkflowCommand(
        WorkflowId workflowId,
        string? name,
        string? description,
        bool? isActive,
        EmbeddingProviderConfigDTO? embeddingProviderConfig,
        ConversationProviderConfigDTO? conversationProviderConfig
    ) : UserAwareCommand<WorkflowDetailsDTO>
    {
        public WorkflowId WorkflowId { get; } = workflowId;
        public string? Name { get; } = name;
        public string? Description { get; } = description;
        public bool? IsActive { get; } = isActive;
        public EmbeddingProviderConfigDTO? EmbeddingProviderConfig { get; } = embeddingProviderConfig;
        public ConversationProviderConfigDTO? ConversationProviderConfig { get; } = conversationProviderConfig;
    }
}