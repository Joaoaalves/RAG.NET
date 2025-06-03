using RAGNET.Domain.SharedKernel.Models;

namespace RAGNET.Domain.SharedKernel.Providers
{
    public interface IProviderModelCatalogService
    {
        Dictionary<SupportedProvider, List<ConversationModel>> GetConversationModels();
        Dictionary<SupportedProvider, List<EmbeddingModel>> GetEmbeddingModels();
    }
}