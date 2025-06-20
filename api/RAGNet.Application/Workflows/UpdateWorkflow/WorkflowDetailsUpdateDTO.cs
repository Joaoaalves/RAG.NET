using RAGNET.Application.Providers.Conversation;
using RAGNET.Application.Providers.Embedding;

namespace RAGNET.Application.Workflows.UpdateWorkflow
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