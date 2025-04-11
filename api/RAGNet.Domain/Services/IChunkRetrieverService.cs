using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Services
{
    public interface IChunkRetrieverService
    {
        Task<List<ContentItem>> RetrieveContent(List<VectorQueryResult> queryResults, bool parentChild = false);
    }
}