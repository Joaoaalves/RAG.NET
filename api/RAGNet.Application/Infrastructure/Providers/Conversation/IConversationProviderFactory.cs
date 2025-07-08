using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Application.Infrastructure.Providers.Conversation
{
    public interface IConversationProviderFactory
    {
        IConversationProviderService CreateCompletionService(ApiKey userApiKey, ConversationProviderConfig config);
    }
}