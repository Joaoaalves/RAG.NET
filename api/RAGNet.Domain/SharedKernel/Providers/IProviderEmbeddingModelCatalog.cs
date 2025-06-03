using RAGNET.Domain.SharedKernel.Models;

namespace RAGNET.Domain.SharedKernel.Providers
{

    public interface IProviderEmbeddingModelCatalog
    {
        List<EmbeddingModel> GetModels();
    }
}