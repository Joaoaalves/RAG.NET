using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers.Embedding
{
    public interface IEmbedderFactory
    {
        IEmbeddingService CreateEmbeddingService(string userApiKey, EmbeddingProviderConfig config);
    }
}