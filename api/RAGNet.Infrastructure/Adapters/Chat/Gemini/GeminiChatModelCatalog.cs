using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Adapters.Chat.Gemini
{
    public class GeminiChatModelCatalog : IProviderConversationModelCatalog
    {
        public List<ConversationModel> GetModels()
        {
            return
            [
                new ConversationModel
                {
                    Label = "Gemini 2.5 Flash Preview",
                    Value = "gemini-2.5-flash-preview-04-17",
                    Speed = 3,
                    InputPrice = 0.15f,
                    OutputPrice = 0.6f,
                    MaxOutput = 65536,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 2.5 Pro Preview",
                    Value = "gemini-2.5-pro-preview-03-25",
                    Speed = 2,
                    InputPrice = 2.5f,
                    OutputPrice = 15,
                    MaxOutput = 65536,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 2.0 Flash",
                    Value = "gemini-2.0-flash",
                    Speed = 5,
                    InputPrice = 0.1f,
                    OutputPrice = 0.4f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 2.0 Flash-Lite",
                    Value = "gemini-2.0-flash-lite",
                    Speed = 5,
                    InputPrice = 0.075f,
                    OutputPrice = 0.3f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 1.5 Flash",
                    Value = "gemini-1.5-flash",
                    Speed = 4,
                    InputPrice = 0.15f,
                    OutputPrice = 0.6f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 1.5 Flash-8B",
                    Value = "gemini-1.5-flash-8b",
                    Speed = 5,
                    InputPrice = 0.075f,
                    OutputPrice = 0.3f,
                    MaxOutput = 8192,
                    ContextWindow = 1_048_576
                },
                new ConversationModel
                {
                    Label = "Gemini 1.5 Pro",
                    Value = "gemini-1.5-pro",
                    Speed = 4,
                    InputPrice = 2.5f,
                    OutputPrice = 10,
                    MaxOutput = 8192,
                    ContextWindow = 2_097_152
                }
            ];
        }
    }
}