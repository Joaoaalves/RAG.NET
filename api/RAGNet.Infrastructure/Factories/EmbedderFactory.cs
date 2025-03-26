using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Embedding;

namespace RAGNET.Infrastructure.Adapters.OpenAIFactory
{
    public class EmbedderFactory : IEmbedderFactory
    {
        public IEmbeddingService CreateEmbeddingService(string apiKey, string? model = null)
        {
            model ??= "text-embedding-ada-002";
            return new OpenAIEmbeddingAdapter(apiKey, model);
        }

    }
}