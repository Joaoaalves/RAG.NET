using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers.Conversation
{
    public interface IChatCompletionFactory
    {
        IChatCompletionService CreateCompletionService(string userApiKey, ConversationProviderConfig config);
    }
}