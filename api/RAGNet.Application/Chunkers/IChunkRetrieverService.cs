using RAGNET.Application.Providers;
using RAGNET.Application.Query;


namespace RAGNET.Application.Chunkers
{
    public interface IChunkRetrieverService
    {
        Task<List<ContentItem>> RetrieveContent(List<VectorQueryResult> queryResults, bool parentChild = false);
    }
}