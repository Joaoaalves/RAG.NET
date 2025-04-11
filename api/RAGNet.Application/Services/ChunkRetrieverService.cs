using RAGNET.Application.Mappers;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Repositories;
using RAGNET.Domain.Services;

namespace RAGNET.Application.Services
{
    public class ChunkRetrieverService(
        IPageRepository pageRepository,
        IChunkRepository chunkRepository
    ) : IChunkRetrieverService
    {
        private readonly IPageRepository _pageRepository = pageRepository;
        private readonly IChunkRepository _chunkRepository = chunkRepository;

        public async Task<List<ContentItem>> RetrieveContent(List<VectorQueryResult> queryResults, bool parentChild = false)
        {
            // Se parentChild for verdadeiro, retornamos páginas. Caso contrário, retornamos apenas chunks.
            if (parentChild)
                return await RetrievePages(queryResults);

            return await RetrieveChunks(queryResults);
        }

        // Método para mapear chunks
        private async Task<List<ContentItem>> RetrieveChunks(List<VectorQueryResult> queryResults)
        {
            var chunks = await GetChunks(queryResults);
            var chunksWithScore = MapChunksByScore(chunks, queryResults);

            // Retorna a lista de ContentItem com base em chunks
            return chunksWithScore.Select(chunk => chunk.ToContentItem()).ToList();
        }

        // Método para mapear páginas
        private async Task<List<ContentItem>> RetrievePages(List<VectorQueryResult> queryResults)
        {
            var chunks = await GetChunks(queryResults);
            var chunksMapped = MapChunksByScore(chunks, queryResults);

            // Recupera as páginas a partir dos chunks
            var pages = await GetPages(chunksMapped);

            // Retorna a lista de ContentItem com base nas páginas
            return pages.Select(page => page.ToContentItem(chunksMapped.FirstOrDefault(chunk => chunk.PageId == page.Id)?.Score ?? 0)).ToList();
        }

        // Método para mapear scores nos chunks
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

        // Recupera os chunks baseados no VectorId
        private async Task<List<Chunk>> GetChunks(List<VectorQueryResult> queryResults)
        {
            var vectorIds = queryResults.Select(q => q.VectorId).ToArray();
            return await _chunkRepository.GetManyByVectorId(vectorIds);
        }

        // Recupera as páginas baseadas nos chunks
        private async Task<List<Page>> GetPages(List<Chunk> chunks)
        {
            var pageIds = chunks.Select(c => c.PageId).ToArray();
            return await _pageRepository.GetMany(pageIds);
        }


    }

}