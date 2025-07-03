using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Embedders.Mistral
{
    public class MistralEmbeddingModelCatalog : IProviderEmbeddingModelCatalog
    {
        public List<EmbeddingModel> GetModels()
        {
            return [
                new() { Label = "Mistral Embed", Value = "mistral-embed", Speed = 7, Price = 0.1f, MaxContext = 8191, VectorSize = 1024 },
            ];
        }
    }
}