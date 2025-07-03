using OpenAI.Chat;

namespace RAGNET.Infrastructure.Embedders.OpenAI
{
    public interface IOpenAIEmbeddingWrapper
    {
        Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    }
}
