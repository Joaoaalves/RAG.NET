using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;

namespace RAGNET.Application.Workflows.Commands.UpdateWorkflow
{
    public class WorkflowDetailsUpdateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public EmbeddingProviderConfigDTO? EmbeddingProvider { get; set; }
        public ConversationProviderConfigDTO? ConversationProvider { get; set; }
    }
}