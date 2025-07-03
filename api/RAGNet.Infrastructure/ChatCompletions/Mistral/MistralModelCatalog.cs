using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.ChatCompletions.Mistral
{
    public class MistralChatModelCatalog : IProviderConversationModelCatalog
    {
        public List<ConversationModel> GetModels()
        {
            return [
                new ConversationModel
                {
                    Label = "Magistral Small",
                    Value = "magistral-small-latest",
                    Speed = 6,
                    InputPrice = 0.5f,
                    OutputPrice = 1.5f,
                    MaxOutput = 4096,
                    ContextWindow = 40000
                },
                new ConversationModel
                {
                    Label = "Magistral Medium",
                    Value = "magistral-medium-latest",
                    Speed = 6,
                    InputPrice = 2f,
                    OutputPrice = 5f,
                    MaxOutput = 4096,
                    ContextWindow = 40000
                },
                new ConversationModel
                {
                    Label = "Mistral Small 3.1",
                    Value = "magistral-small-2503",
                    Speed = 6,
                    InputPrice = 0.1f,
                    OutputPrice = 0.3f,
                    MaxOutput = 8192,
                    ContextWindow = 128000
                },
                new ConversationModel
                {
                    Label = "Mistral Small 3.2",
                    Value = "magistral-small-2506",
                    Speed = 6,
                    InputPrice = 0.1f,
                    OutputPrice = 0.3f,
                    MaxOutput = 8192,
                    ContextWindow = 128000
                },
                new ConversationModel
                {
                    Label = "Mistral Medium 3",
                    Value = "mistral-medium-latest",
                    Speed = 5,
                    InputPrice = 0.4f,
                    OutputPrice = 2f,
                    MaxOutput = 8192,
                    ContextWindow = 128000
                },
            ];
        }
    }
}