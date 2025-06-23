using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers.Conversation
{
    public interface IConversationProviderResolver
    {
        ConversationModel Resolve(ConversationProviderConfig config);
    }
}