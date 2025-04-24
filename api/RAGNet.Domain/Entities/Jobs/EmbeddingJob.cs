namespace RAGNET.Domain.Entities.Jobs
{
    public class EmbeddingJob
    {
        public Guid JobId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; }
        public string FileName { get; set; } = String.Empty;
        public byte[] FileContent { get; set; } = [];
        public List<CallbackUrl> CallbackUrls { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}