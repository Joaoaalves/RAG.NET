using RAGNET.Domain.Entities;

namespace RAGNET.Application.DTOs.Conversation
{
    public class ConversationModelsDTO
    {
        public List<ConversationModel>? Anthropic { get; set; } = [];
        public List<ConversationModel>? OpenAI { get; set; } = [];
        public List<ConversationModel>? Gemini { get; set; } = [];
    }
}