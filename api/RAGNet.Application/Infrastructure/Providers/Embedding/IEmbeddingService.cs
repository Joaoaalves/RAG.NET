using RAGNET.Domain.SharedKernel.VectorStorages.SemanticVectors;

namespace RAGNET.Application.Infrastructure.Providers.Embedding
{
    public interface IEmbeddingService
    {
        Task<SemanticVector> GetEmbeddingAsync(string text);
        Task<List<SemanticVector>> GetMultipleEmbeddingAsync(List<string> texts);
    }
}