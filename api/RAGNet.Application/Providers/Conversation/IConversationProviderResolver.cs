using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers.Conversation
{
    public interface IConversationProviderResolver
    {
        ConversationModel Resolve(ConversationProviderConfig config);
    }
}