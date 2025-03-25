using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Infrastructure.Data;

namespace RAGNET.Infrastructure.Repositories
{
    public class ChunkRepository(ApplicationDbContext context) : Repository<Chunk>(context), IChunkRepository { }
}