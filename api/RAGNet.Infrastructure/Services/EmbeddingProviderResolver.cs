using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Services;

using RAGNET.Infrastructure.Adapters.Embedding;


namespace RAGNET.Infrastructure.Services
{
    public class EmbeddingProviderResolver : IEmbeddingProviderResolver
    {

        public EmbeddingModel Resolve(EmbeddingProviderConfig config)
        {
            List<EmbeddingModel> validModels = [];
            if (config.Provider == EmbeddingProviderEnum.OPENAI)
            {
                validModels = OpenAIEmbeddingAdapter.GetModels();
            }

            if (config.Provider == EmbeddingProviderEnum.VOYAGE)
            {
                validModels = VoyageEmbeddingAdapter.GetModels();
            }

            if (validModels.Count == 0)
            {
                throw new InvalidEmbeddingModelException(
                    $"This embedding provider '{config.Provider}' is not valid."
                );
            }

            EmbeddingModel validModel = validModels.FirstOrDefault(m => m.Value == config.Model) ?? throw new InvalidEmbeddingModelException(
                    $"This embedding model '{config.Model}' is not valid."
                );

            return validModel;
        }
    }
}
