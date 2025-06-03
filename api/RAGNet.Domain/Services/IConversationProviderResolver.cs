using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Models;

namespace RAGNET.Domain.Services
{
    public interface IConversationProviderResolver
    {
        ConversationModel Resolve(ConversationProviderConfig config);
    }
}