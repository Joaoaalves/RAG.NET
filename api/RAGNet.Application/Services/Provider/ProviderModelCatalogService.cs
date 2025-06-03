using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Services.Provider
{
    public class ProviderModelCatalogService(
        Dictionary<SupportedProvider, IProviderConversationModelCatalog> conversationProviders,
        Dictionary<SupportedProvider, IProviderEmbeddingModelCatalog> embeddingProviders) : IProviderModelCatalogService
    {
        private readonly Dictionary<SupportedProvider, IProviderConversationModelCatalog> _conversationProviders = conversationProviders;
        private readonly Dictionary<SupportedProvider, IProviderEmbeddingModelCatalog> _embeddingProviders = embeddingProviders;

        public Dictionary<SupportedProvider, List<ConversationModel>> GetConversationModels()
        {
            return _conversationProviders.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.GetModels());
        }

        public Dictionary<SupportedProvider, List<EmbeddingModel>> GetEmbeddingModels()
        {
            return _embeddingProviders.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.GetModels());
        }
    }
}