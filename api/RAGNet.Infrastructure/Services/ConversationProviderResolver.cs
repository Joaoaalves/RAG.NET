using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Services;

using RAGNET.Infrastructure.Adapters.Chat;

namespace RAGNET.Infrastructure.Services
{
    public class ConversationProviderResolver : IConversationProviderResolver
    {

        public ConversationModel Resolve(ConversationProviderConfig config)
        {
            List<ConversationModel> validModels = [];
            if (config.Provider == ConversationProviderEnum.OPENAI)
            {
                validModels = OpenAIChatAdapter.GetModels();
            }

            if (config.Provider == ConversationProviderEnum.ANTHROPIC)
            {
                validModels = AnthropicChatAdapter.GetModels();
            }

            if (config.Provider == ConversationProviderEnum.GEMINI)
            {
                validModels = GeminiChatAdapter.GetModels();
            }

            ConversationModel validModel = validModels.FirstOrDefault(m => m.Value == config.Model) ?? throw new InvalidConversationModelException(
                    $"The model '{config.Model}' is not valid."
                );

            return validModel;
        }
    }
}
