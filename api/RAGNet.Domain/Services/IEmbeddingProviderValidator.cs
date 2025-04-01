using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IEmbeddingProviderValidator
    {
        void Validate(EmbeddingProviderConfig config);
    }
}