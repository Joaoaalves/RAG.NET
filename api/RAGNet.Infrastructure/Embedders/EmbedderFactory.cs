using RAGNET.Application.Infrastructure.Providers.Embedding;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Users.ApiKeys;
using RAGNET.Infrastructure.Embedders.Gemini;
using RAGNET.Infrastructure.Embedders.Mistral;
using RAGNET.Infrastructure.Embedders.OpenAI;
using RAGNET.Infrastructure.Embedders.Voyage;

namespace RAGNET.Infrastructure.Embedders
{
    public class EmbedderFactory : IEmbedderFactory
    {
        private readonly int _baseDelayMS = 1000;
        public IEmbeddingService CreateEmbeddingService(ApiKey userApiKey, EmbeddingProviderConfig config)
        {
            var model = config.Model;
            return config.Provider switch
            {
                EmbeddingProviderEnum.OPENAI => OpenAIClient(userApiKey, model),
                EmbeddingProviderEnum.VOYAGE => VoyageClient(userApiKey, model),
                EmbeddingProviderEnum.GEMINI => GeminiClient(userApiKey, model),
                EmbeddingProviderEnum.MISTRAL => MistralClient(userApiKey, model),
                _ => throw new NotSupportedException("Embedding provider not supported.")
            };
        }

        private OpenAIEmbeddingAdapter OpenAIClient(ApiKey apiKey, string model)
        {
            var embeddingClient = new OpenAIEmbeddingClientWrapper(apiKey.Value, model);

            return new OpenAIEmbeddingAdapter(
                embeddingClient,
                delayMs: _baseDelayMS
            );
        }

        private VoyageEmbeddingAdapter VoyageClient(ApiKey apiKey, string model)
        {
            return new VoyageEmbeddingAdapter(apiKey.Value, model, delayMs: _baseDelayMS);
        }

        private GeminiEmbeddingAdapter GeminiClient(ApiKey apiKey, string model)
        {
            return new GeminiEmbeddingAdapter(apiKey.Value, model, delayMs: _baseDelayMS);
        }

        private MistralEmbeddingAdapter MistralClient(ApiKey apiKey, string model)
        {
            return new MistralEmbeddingAdapter(apiKey.Value, model, delayMs: _baseDelayMS);
        }
    }
}