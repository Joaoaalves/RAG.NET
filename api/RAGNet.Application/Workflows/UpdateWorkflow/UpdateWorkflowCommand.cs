using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Providers.Conversation;
using RAGNET.Application.Providers.Embedding;
using RAGNET.Application.Workflows.GetWorkflowDetails;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Workflows.UpdateWorkflow
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