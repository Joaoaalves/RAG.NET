using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Documents.Pages;
using RAGNET.Application.UserQueries;

namespace RAGNET.Application.Mappers
{
    public static class ContentItemMapper
    {
        public static ContentItem ToContentItem(this Chunk chunk)
        {
            return new ContentItem
            {
                Id = chunk.Id.Value,
                Text = chunk.Text.Value,
                Score = chunk.GetScore(),
                Source = ContentSourceEnum.CHUNK,
                ChunkId = chunk.Id
            };
        }

        public static ContentItem ToContentItem(this Page page, double score)
        {
            return new ContentItem
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