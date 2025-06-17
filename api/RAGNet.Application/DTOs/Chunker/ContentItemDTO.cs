using RAGNET.Application.Query;

namespace RAGNET.Application.DTOs.Chunker
{
    public class ContentItemDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public double Score { get; set; }
        public ContentSourceEnum Source { get; set; }
        public Guid? PageId { get; set; }
        public Guid? ChunkId { get; set; }
    }
}