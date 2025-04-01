using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Embedding;

namespace RAGNET.Infrastructure.Factories
{
    public class EmbedderFactory : IEmbedderFactory
    {
        public IEmbeddingService CreateEmbeddingService(string apiKey, string model, EmbeddingProviderEnum provider)
        {

            return provider switch
            {
                EmbeddingProviderEnum.OPENAI => new OpenAIEmbeddingAdapter(apiKey, model),
                EmbeddingProviderEnum.VOYAGE => new VoyageEmbeddingAdapter(apiKey, model),
                _ => throw new NotSupportedException("Embedding provider not supported.")
            };
        }

    }
}