using RAGNET.Application.Providers;

using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Infrastructure.Embedders
{
    public class EmbedderFactory : IEmbedderFactory
    {
        public IEmbeddingService CreateEmbeddingService(string userApiKey, EmbeddingProviderConfig config)
        {
            return config.Provider switch
            {
                EmbeddingProviderEnum.OPENAI => new OpenAIEmbeddingAdapter(userApiKey, config.Model),
                EmbeddingProviderEnum.VOYAGE => new VoyageEmbeddingAdapter(userApiKey, config.Model),
                EmbeddingProviderEnum.GEMINI => new GeminiEmbeddingAdapter(userApiKey, config.Model),
                _ => throw new NotSupportedException("Embedding provider not supported.")
            };
        }

    }
}