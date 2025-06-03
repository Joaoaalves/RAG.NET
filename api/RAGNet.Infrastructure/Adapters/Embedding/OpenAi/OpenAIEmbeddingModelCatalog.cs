using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Adapters.Embedding.OpenAI
{
    public class OpenAIEmbeddingModelCatalog : IProviderEmbeddingModelCatalog
    {
        public List<EmbeddingModel> GetModels()
        {
            return [
                new() { Label = "Embedding 3 Large", Value = "text-embedding-3-large", Speed = 4, Price = 0.13f, MaxContext = 8191, VectorSize = 1536 },
                new() { Label = "Embedding 3 Small", Value = "text-embedding-3-small", Speed = 5, Price = 0.02f, MaxContext = 8191, VectorSize = 1536 },
                new() { Label = "Embedding Ada 002", Value = "text-embedding-ada-002", Speed = 3, Price = 0.10f, MaxContext = 8191, VectorSize = 1536 },
            ];
        }
    }
}