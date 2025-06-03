using RAGNET.Application.DTOs.Embedding;
using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Mappers
{
    public static class EmbeddingProviderMapper
    {
        public static EmbeddingProviderConfig ToEmbeddingProviderConfig(this EmbeddingProviderConfigDTO dto, Guid workflowId, int vectorSize = 0)
        {
            return new EmbeddingProviderConfig
            {
                Provider = dto.ProviderId,
                Model = dto.Model,
                VectorSize = vectorSize,
                WorkflowId = workflowId
            };
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
                EmbeddingProviderEnum.OPENAI => SupportedProvider.OpenAI,
                EmbeddingProviderEnum.VOYAGE => SupportedProvider.Voyage,
                EmbeddingProviderEnum.GEMINI => SupportedProvider.Gemini,
                _ => throw new ArgumentOutOfRangeException("Unsupported provider")
            };
        }
    }
}