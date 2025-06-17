
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Application.Providers
{
    public interface IEmbedderFactory
    {
        IEmbeddingService CreateEmbeddingService(string userApiKey, EmbeddingProviderConfig config);
    }
}