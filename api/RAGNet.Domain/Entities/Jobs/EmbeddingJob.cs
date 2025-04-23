using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities.Jobs
{
    public class EmbeddingJob
    {
        public Guid JobId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; }
        public List<string> Chunks { get; set; } = [];
        public JobStatus Status { get; set; } = JobStatus.PENDING;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}