using RAGNET.Application.Providers;
using RAGNET.Application.UserQueries;


namespace RAGNET.Application.Chunkers
{
    public interface IChunkRetrieverService
    {
        Task<List<ContentItem>> RetrieveContent(List<VectorQueryResult> queryResults, bool parentChild = false);
    }
}