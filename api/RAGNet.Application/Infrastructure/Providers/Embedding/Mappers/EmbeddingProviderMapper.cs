using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Infrastructure.Providers.Embedding.Mappers
{
    public static class EmbeddingProviderMapper
    {
        public static EmbeddingProviderConfig ToEmbeddingProviderConfig(this EmbeddingProviderConfigDTO dto, int vectorSize = 0)
        {
            return new EmbeddingProviderConfig(
                provider: dto.ProviderId,
                model: dto.Model,
                vectorSize: vectorSize);
        }

        public static EmbeddingProviderConfigDTO ToDTOFromEmbeddingProviderConfig(this EmbeddingProviderConfig embeddingProvider)
        {
            return new EmbeddingProviderConfigDTO
            {
                ProviderId = embeddingProvider.Provider,
                ProviderName = embeddingProvider.Provider,
                VectorSize = embeddingProvider.VectorSize,
                Model = embeddingProvider.Model
            };
        }

        public static SupportedProvider ToSupportedProvider(this EmbeddingProviderEnum provider)
        {
            return provider switch
            {
                EmbeddingProviderEnum.OPENAI => SupportedProvider.OPENAI,
                EmbeddingProviderEnum.VOYAGE => SupportedProvider.VOYAGE,
                EmbeddingProviderEnum.GEMINI => SupportedProvider.GEMINI,
                EmbeddingProviderEnum.MISTRAL => SupportedProvider.MISTRAL,
                _ => throw new ArgumentOutOfRangeException("Unsupported provider")
            };
        }
    }
}