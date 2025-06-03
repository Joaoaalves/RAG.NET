using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Adapters.Embedding.Voyage
{
    public class VoyageEmbeddingModelCatalog : IProviderEmbeddingModelCatalog
    {
        public List<EmbeddingModel> GetModels()
        {
            return
            [
                new() { Label = "Voyage 3 Large", Value = "voyage-3-large", Speed = 3, Price = 0.18f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage 3", Value = "voyage-3", Speed = 6, Price = 0.06f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage 3 Lite", Value = "voyage-3-lite", Speed = 7, Price = 0.02f, MaxContext = 32000, VectorSize = 512 },
                new() { Label = "Voyage Code 3", Value = "voyage-code-3", Speed = 4, Price = 0.18f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage Finance 2", Value = "voyage-finance-2", Speed = 4, Price = 0.12f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage Law 2", Value = "voyage-law-2", Speed = 4, Price = 0.12f, MaxContext = 32000, VectorSize = 1024 },
                new() { Label = "Voyage Code 2", Value = "voyage-code-2", Speed = 4, Price = 0.12f, MaxContext = 32000, VectorSize = 1024 }
            ];
        }
    }
}