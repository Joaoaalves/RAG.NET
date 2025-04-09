using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Embedding;

namespace RAGNET.Infrastructure.Factories
{
    public class EmbedderFactory : IEmbedderFactory
    {
        public IEmbeddingService CreateEmbeddingService(EmbeddingProviderConfig config)
        {

            return config.Provider switch
            {
                EmbeddingProviderEnum.OPENAI => new OpenAIEmbeddingAdapter(config.ApiKey, config.Model),
                EmbeddingProviderEnum.VOYAGE => new VoyageEmbeddingAdapter(config.ApiKey, config.Model),
                _ => throw new NotSupportedException("Embedding provider not supported.")
            };
        }

    }
}