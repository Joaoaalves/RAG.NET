using RAGNET.Domain.Entities;
using RAGNET.Domain.Eums;

namespace RAGNET.Application.Mappers
{
    public static class ContentItemMapper
    {
        public static ContentItem ToContentItem(this Chunk chunk)
        {
            return new ContentItem
            {
                Id = chunk.Id,
                Text = chunk.Text,
                Score = chunk.Score,
                Source = ContentSourceEnum.CHUNK,
                ChunkId = chunk.Id
            };
        }

        public static ContentItem ToContentItem(this Page page, double score)
        {
            return new ContentItem
            {
                Id = page.Id,
                Text = page.Text,
                Score = score,
                Source = ContentSourceEnum.PAGE,
                PageId = page.Id
            };
        }
    }
}