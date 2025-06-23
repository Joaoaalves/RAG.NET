using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Queries.DTOs;


namespace RAGNET.Application.Chunkers.Services
{
    public interface IChunkRetrieverService
    {
        Task<List<ContentItemDTO>> RetrieveContent(List<VectorQueryResult> queryResults, bool parentChild = false);
    }
}