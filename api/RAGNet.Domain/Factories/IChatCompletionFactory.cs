using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

namespace RAGNET.Domain.Factories
{
    public interface IChatCompletionFactory
    {
        IChatCompletionService CreateCompletionService(string userApiKey, ConversationProviderConfig config);
    }
}