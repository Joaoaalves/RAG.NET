using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Chat;

namespace RAGNET.Infrastructure.Services
{
    public class ConversationProviderValidator : IConversationProviderValidator
    {

        public void Validate(ConversationProviderConfig config)
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

            bool isValid = validModels.Any(
                m => m.Value.Equals(config.Model, StringComparison.OrdinalIgnoreCase));

            if (!isValid)
            {
                throw new InvalidConversationModelException(
                    $"This embedding model '{config.Model}' is not valid."
                );
            }
        }
    }
}
