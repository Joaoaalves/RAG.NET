using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Services;

namespace RAGNET.Domain.Factories
{
    public interface IEmbedderFactory
    {
        IEmbeddingService CreateEmbeddingService(EmbeddingProviderConfig config);
    }
}