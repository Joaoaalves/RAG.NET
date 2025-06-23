using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Application.Queries.DTOs;

namespace RAGNET.Application.Queries.Mappers
{
    public static class ContentItemMapper
    {
        public static ContentItemDTO ToContentItem(this Chunk chunk)
        {
            return new ContentItemDTO
            {
                Id = chunk.Id.Value,
                Text = chunk.Text.Value,
                Score = chunk.GetScore(),
                Source = ContentSourceEnum.CHUNK,
                ChunkId = chunk.Id
            };
        }

        public static ContentItemDTO ToContentItem(this Page page, double score)
        {
            return new ContentItemDTO
            {
                Id = page.Id.Value,
                Text = page.Text.Value,
                Score = score,
                Source = ContentSourceEnum.PAGE,
                PageId = page.Id
            };
        }
    }
}