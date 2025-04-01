using RAGNET.Application.DTOs.Embedder;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.Mappers
{
    public static class EmbeddingProviderMapper
    {
        public static EmbeddingProviderConfig ToEmbeddingProviderConfigFromEmbeddingProviderConfigDTO(this EmbeddingProviderConfigDTO dto, Guid workflowId)
        {
            return new EmbeddingProviderConfig
            {
                Provider = dto.Provider,
                ApiKey = dto.ApiKey,
                Model = dto.Model,
                VectorSize = dto.VectorSize,
                WorkflowId = workflowId
            };
        }

        public static EmbeddingProviderConfigDTO ToDTOFromEmbeddingProviderConfig(this EmbeddingProviderConfig embeddingProvider)
        {
            return new EmbeddingProviderConfigDTO
            {
                Provider = embeddingProvider.Provider,
                ApiKey = embeddingProvider.ApiKey,
                VectorSize = embeddingProvider.VectorSize
            };
        }
    }
}