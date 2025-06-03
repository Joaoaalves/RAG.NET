using Anthropic.SDK.Constants;
using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Adapters.Chat.Anthropic
{
    public class AnthropicChatModelCatalog : IProviderConversationModelCatalog
    {
        public List<ConversationModel> GetModels()
        {
            return
             [
                 new ConversationModel
                {
                    Label = "Claude 3.7 Sonnet",
                    Value = AnthropicModels.Claude37Sonnet,
                    Speed = 4,
                    InputPrice = 3,
                    OutputPrice = 15,
                    MaxOutput = 8192,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3.5 Sonnet",
                    Value = AnthropicModels.Claude35Sonnet,
                    Speed = 5,
                    InputPrice = 3,
                    OutputPrice = 15,
                    MaxOutput = 8192,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3.5 Haiku",
                    Value = AnthropicModels.Claude35Haiku,
                    Speed = 7,
                    InputPrice = 0.8f,
                    OutputPrice = 15,
                    MaxOutput = 8192,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3 Opus",
                    Value = AnthropicModels.Claude3Opus,
                    Speed = 6,
                    InputPrice = 15,
                    OutputPrice = 75,
                    MaxOutput = 4096,
                    ContextWindow = 200000
                },
                new ConversationModel
                {
                    Label = "Claude 3 Haiku",
                    Value = AnthropicModels.Claude3Haiku,
                    Speed = 8,
                    InputPrice = 0.25f,
                    OutputPrice = 1.25f,
                    MaxOutput = 4096,
                    ContextWindow = 200000
                },
            ];
        }
    }
}