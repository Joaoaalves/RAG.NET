using RAGNET.Domain.Eums;

namespace RAGNET.Domain.Entities
{
    public class ContentItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public double Score { get; set; }
        public ContentSourceEnum Source { get; set; }
        public Guid? PageId { get; set; }
        public Guid? ChunkId { get; set; }
    }
}