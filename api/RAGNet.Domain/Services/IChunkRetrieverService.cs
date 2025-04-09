using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IChunkRetrieverService
    {
        Task<List<Chunk>> RetrieveChunks(List<VectorQueryResult> queryResults);
        Task<List<Page>> RetrievePages(List<VectorQueryResult> queryResults);
    }
}