using OpenAI.Chat;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Infrastructure.Embedders.OpenAI
{
    public interface IOpenAIEmbeddingWrapper
    {
        Task<SemanticVector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    }
}
