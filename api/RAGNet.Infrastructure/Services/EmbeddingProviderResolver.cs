using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Services;
using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;


namespace RAGNET.Infrastructure.Services
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
