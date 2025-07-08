using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.SharedKernel.VectorStorages.Scores;

using RAGNET.Application.Infrastructure.Providers;
using RAGNET.Application.Queries.DTOs;
using RAGNET.Application.Queries.Mappers;

namespace RAGNET.Application.Chunkers.Services
{
    public class ChunkRetrieverService(
        IPageRepository pageRepository,
        IChunkRepository chunkRepository
    ) : IChunkRetrieverService
    {
        private readonly IPageRepository _pageRepository = pageRepository;
        private readonly IChunkRepository _chunkRepository = chunkRepository;

        public async Task<List<ContentItemDTO>> RetrieveContent(List<VectorQueryResult> queryResults, bool parentChild = false)
        {
            // If parentChild is active, retrieve pages first
            if (parentChild)
                return await RetrievePages(queryResults);

            return await RetrieveChunks(queryResults);
        }

        // Map chunks to ContentItem
        private async Task<List<ContentItemDTO>> RetrieveChunks(List<VectorQueryResult> queryResults)
        {
            var chunks = await GetChunks(queryResults);
            var chunksWithScore = MapChunksByScore(chunks, queryResults);

            // Return the list of ContentItem based on chunks
            return chunksWithScore.Select(chunk => chunk.ToContentItem()).ToList();
        }

        // Map pages to ContentItem
        private async Task<List<ContentItemDTO>> RetrievePages(List<VectorQueryResult> queryResults)
        {
            var chunks = await GetChunks(queryResults);
            var chunksMapped = MapChunksByScore(chunks, queryResults);

            // Recupera as páginas a partir dos chunks
            var pages = await GetPages(chunksMapped);

            // Retorna a lista de ContentItem com base nas páginas
            return pages.Select(page => page.ToContentItem(chunksMapped.FirstOrDefault(chunk => chunk.PageId == page.Id)?.GetScore() ?? 0)).ToList();
        }

        // Map chunks by score based on query results
        private List<Chunk> MapChunksByScore(List<Chunk> chunks, List<VectorQueryResult> queryResults)
        {
            return chunks.Select(chunk =>
            {
                var queryResult = queryResults.FirstOrDefault(q => q.VectorId == chunk.VectorId);
                if (queryResult != null)
                {
                    var score = new Score(queryResult.Score);
                    chunk.SetScore(score);
                }
                return chunk;
            })
            .OrderByDescending(chunk => chunk.GetScore())
            .ToList();
        }

        // Retrieve chunks based on query results
        private async Task<List<Chunk>> GetChunks(List<VectorQueryResult> queryResults)
        {
            var vectorIds = queryResults.Select(q => q.VectorId).ToArray();
            return await _chunkRepository.GetManyByVectorId(vectorIds);
        }

        // Get pages based on chunks
        private async Task<List<Page>> GetPages(List<Chunk> chunks)
        {
            var pageIds = chunks.Select(c => c.PageId).ToArray();
            return await _pageRepository.GetManyAsync(pageIds);
        }

    }

}