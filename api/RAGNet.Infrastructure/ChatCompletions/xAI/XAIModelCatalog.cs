using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.ChatCompletions.xAI
{
    public class XAIChatModelCatalog : IProviderConversationModelCatalog
    {
        public List<ConversationModel> GetModels()
        {
            return [
                new ConversationModel{
                    Label = "Grok 3",
                    Value = "grok-3-latest",
                    Speed = 3,
                    InputPrice = 3f,
                    OutputPrice = 15f,
                    MaxOutput = 16000,
                    ContextWindow = 131072
                },
                new ConversationModel{
                    Label = "Grok 3 Fast",
                    Value = "grok-3-fast-latest",
                    Speed = 4,
                    InputPrice = 5f,
                    OutputPrice = 25f,
                    MaxOutput = 16000,
                    ContextWindow = 131072
                },
                new ConversationModel{
                    Label = "Grok 3 Mini",
                    Value = "grok-3-mini-latest",
                    Speed = 4,
                    InputPrice = 0.3f,
                    OutputPrice = 0.5f,
                    MaxOutput = 16000,
                    ContextWindow = 131072
                },
                new ConversationModel{
                    Label = "Grok 3 Mini Fast",
                    Value = "grok-3-mini-fast-latest",
                    Speed = 5,
                    InputPrice = 0.6f,
                    OutputPrice = 4f,
                    MaxOutput = 16000,
                    ContextWindow = 131072
                },
                new ConversationModel{
                    Label = "Grok 2",
                    Value = "grok-2-latest",
                    Speed = 3,
                    InputPrice = 2f,
                    OutputPrice = 10f,
                    MaxOutput = 16000,
                    ContextWindow = 131072
                },
            ];
        }
    }
}