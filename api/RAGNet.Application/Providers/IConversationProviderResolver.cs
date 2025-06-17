using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers
{
    public interface IConversationProviderResolver
    {
        ConversationModel Resolve(ConversationProviderConfig config);
    }
}