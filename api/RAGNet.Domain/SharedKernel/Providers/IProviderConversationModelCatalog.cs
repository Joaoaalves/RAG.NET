using RAGNET.Domain.SharedKernel.Models;

namespace RAGNET.Domain.SharedKernel.Providers
{
    public interface IProviderConversationModelCatalog
    {
        List<ConversationModel> GetModels();
    }
}