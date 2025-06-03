using RAGNET.Domain.SharedKernel.Models;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Adapters.Embedding.Gemini
{
    public class GeminiEmbeddingModelCatalog : IProviderEmbeddingModelCatalog
    {
        public List<EmbeddingModel> GetModels()
        {
            return [
                new() {
                    Label = "Gemini Embedding Experimental",
                    Value = "gemini-embedding-exp-03-07",
                    Speed = 4,
                    Price = 0,
                    MaxContext = 8192,
                    VectorSize = 3072
                },
                new() {
                    Label = "Text Embedding",
                    Value = "text-embedding-004",
                    Speed = 4,
                    Price = 0,
                    MaxContext = 8192,
                    VectorSize = 768
                },
            ];
        }
    }
}