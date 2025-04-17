using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IEmbeddingProviderResolver
    {
        EmbeddingModel Resolve(EmbeddingProviderConfig config);
    }
}