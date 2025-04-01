using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IConversationProviderValidator
    {
        void Validate(ConversationProviderConfig config);
    }
}