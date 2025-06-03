using RAGNET.Domain.Entities;
using RAGNET.Domain.Factories;
using RAGNET.Domain.Services;
using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Infrastructure.Adapters.Embedding.Gemini;
using RAGNET.Infrastructure.Adapters.Embedding.OpenAI;
using RAGNET.Infrastructure.Adapters.Embedding.Voyage;

namespace RAGNET.Infrastructure.Factories
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