using RAGNET.Domain.SharedKernel.VectorStorages.SemanticVectors;

namespace RAGNET.Infrastructure.Embedders.OpenAI
{
    public interface IOpenAIEmbeddingWrapper
    {
        Task<SemanticVector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    }
}
