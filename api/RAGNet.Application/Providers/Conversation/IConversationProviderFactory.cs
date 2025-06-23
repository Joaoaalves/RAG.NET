using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers.Conversation
{
    public interface IConversationProviderFactory
    {
        IConversationProviderService CreateCompletionService(string userApiKey, ConversationProviderConfig config);
    }
}