using RAGNET.Application.DTOs.Conversation;
using RAGNET.Application.DTOs.Embedding;

namespace RAGNET.Application.DTOs.Workflow
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