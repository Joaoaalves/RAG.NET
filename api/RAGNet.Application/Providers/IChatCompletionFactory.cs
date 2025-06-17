using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers
{
    public interface IChatCompletionFactory
    {
        IChatCompletionService CreateCompletionService(string userApiKey, ConversationProviderConfig config);
    }
}