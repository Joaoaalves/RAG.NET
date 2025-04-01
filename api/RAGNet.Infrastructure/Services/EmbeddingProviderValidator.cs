using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Exceptions;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Embedding;

namespace RAGNET.Infrastructure.Services
{
    public class EmbeddingProviderValidator : IEmbeddingProviderValidator
    {

        public void Validate(EmbeddingProviderConfig config)
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

            bool isValid = validModels.Any(m => m.Value.Equals(config.Model, StringComparison.OrdinalIgnoreCase) && m.VectorSize == config.VectorSize);

            if (!isValid)
            {
                throw new InvalidEmbeddingModelException($"This embedding model '{config.Model}' with vectorSize of {config.VectorSize} is not valid.");
            }
        }
    }
}
