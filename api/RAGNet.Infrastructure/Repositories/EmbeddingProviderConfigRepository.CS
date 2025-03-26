using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class EmbeddingProviderConfigRepository(ApplicationDbContext context) : Repository<EmbeddingProviderConfig>(context), IEmbeddingProviderConfigRepository
    {
    }
}