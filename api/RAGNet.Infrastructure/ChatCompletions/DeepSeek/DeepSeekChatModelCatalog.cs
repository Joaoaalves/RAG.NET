using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.ChatCompletions.DeepSeek
{
    public class DeepSeekChatModelCatalog : IProviderConversationModelCatalog
    {
        public List<ConversationModel> GetModels()
        {
            return [
                new ConversationModel
                {
                    Label = "DeepSeek Chat",
                    Value = "deepseek-chat",
                    Speed = 3,
                    InputPrice = 0.27f,
                    OutputPrice = 1.1f,
                    MaxOutput=8192,
                    ContextWindow = 64000
                },
                new ConversationModel
                {
                    Label = "DeepSeek Reasoner",
                    Value = "deepseek-reasoner",
                    Speed = 2,
                    InputPrice = 0.55f,
                    OutputPrice = 2.19f,
                    MaxOutput=64000,
                    ContextWindow = 64000
                }
            ];
        }
    }
}