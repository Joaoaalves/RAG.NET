using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Application.Providers;

using RAGNET.Infrastructure.Exceptions;

namespace RAGNET.Infrastructure.Embedders
{
    public class EmbeddingProviderResolver(IProviderModelCatalogService providerModelCatalogService) : IEmbeddingProviderResolver
    {
        private readonly IProviderModelCatalogService _providerModelCatalogService = providerModelCatalogService;
        public EmbeddingModel Resolve(EmbeddingProviderConfig config)
        {
            Dictionary<SupportedProvider, List<EmbeddingModel>> validModels = _providerModelCatalogService.GetEmbeddingModels();

            var provider = (SupportedProvider)config.Provider;

            if (validModels.TryGetValue(provider, out var models))
            {
                var validModel = models.FirstOrDefault(m => m.Value == config.Model) ?? throw new InvalidConversationModelException($"The model '{config.Model}' is not valid for provider '{config.Provider}'.");
                return validModel;
            }

            throw new InvalidConversationModelException($"The provider '{config.Provider}' is not supported.");

        }
    }
}
