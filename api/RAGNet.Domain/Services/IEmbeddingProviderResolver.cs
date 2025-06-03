using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Models;

namespace RAGNET.Domain.Services
{
    public interface IEmbeddingProviderResolver
    {
        EmbeddingModel Resolve(EmbeddingProviderConfig config);
    }
}