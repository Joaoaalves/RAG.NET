using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.Documents.Pages.Chunks;

namespace RAGNET.Application.Queries.DTOs
{
    public class ContentItemDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public double Score { get; set; }
        public ContentSourceEnum Source { get; set; }
        public PageId? PageId { get; set; }
        public ChunkId? ChunkId { get; set; }
    }
}