using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IConversationProviderResolver
    {
        ConversationModel Resolve(ConversationProviderConfig config);
    }
}