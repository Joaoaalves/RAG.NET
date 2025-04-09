using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.Services
{
    public class ChunkRetrieverService(IPageRepository pageRepository, IChunkRepository chunkRepository) : IChunkRetrieverService
    {
        private readonly IPageRepository _pageRepository = pageRepository;
        private readonly IChunkRepository _chunkRepository = chunkRepository;

        public async Task<List<Chunk>> RetrieveChunks(List<VectorQueryResult> queryResults)
        {
            var chunks = await GetChunks(queryResults);

            var chunksWithScore = MapChunksByScore(chunks, queryResults);

            return chunksWithScore;
        }

        public async Task<List<Page>> RetrievePages(List<VectorQueryResult> queryResults)
        {
            var chunks = await GetChunks(queryResults);
            var chunksMapped = MapChunksByScore(chunks, queryResults);
            return await GetPages(chunksMapped);
        }

        private List<Chunk> MapChunksByScore(List<Chunk> chunks, List<VectorQueryResult> queryResults)
        {
            return chunks.Select(chunk =>
            {
                var queryResult = queryResults.FirstOrDefault(q => q.VectorId == chunk.VectorId);
                if (queryResult != null)
                {
                    chunk.Score = queryResult.Score;
                }
                return chunk;
            })
            .OrderByDescending(chunk => chunk.Score)
            .ToList();
        }

        // Reduce the queryResults VectorId att for an array and search
        private async Task<List<Chunk>> GetChunks(List<VectorQueryResult> queryResults)
        {
            var vectorIds = queryResults.Select(q => q.VectorId).ToArray();
            return await _chunkRepository.GetManyByVectorId(vectorIds);
        }

        // Reduce the chunks PageId att for an array and search
        private async Task<List<Page>> GetPages(List<Chunk> chunks)
        {
            var pageIds = chunks.Select(c => c.PageId).ToArray();
            return await _pageRepository.GetMany(pageIds);
        }
    }
}