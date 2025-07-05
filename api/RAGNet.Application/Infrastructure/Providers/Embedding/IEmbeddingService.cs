using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Application.Infrastructure.Providers.Embedding
{
    public interface IEmbeddingService
    {
        Task<SemanticVector> GetEmbeddingAsync(string text);
        Task<List<SemanticVector>> GetMultipleEmbeddingAsync(List<string> texts);
    }
}