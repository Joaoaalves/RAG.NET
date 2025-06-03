using RAGNET.Application.DTOs.Embedder;
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
                Provider = dto.Provider,
                Model = dto.Model,
                VectorSize = vectorSize,
                WorkflowId = workflowId
            };
        }

        public static EmbeddingProviderConfigDTO ToDTOFromEmbeddingProviderConfig(this EmbeddingProviderConfig embeddingProvider)
        {
            return new EmbeddingProviderConfigDTO
            {
                Provider = embeddingProvider.Provider,
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