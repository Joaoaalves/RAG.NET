using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.Users.ApiKeys;

namespace RAGNET.Application.Infrastructure.Providers.Embedding
{
    public interface IEmbedderFactory
    {
        IEmbeddingService CreateEmbeddingService(ApiKey userApiKey, EmbeddingProviderConfig config);
    }
}