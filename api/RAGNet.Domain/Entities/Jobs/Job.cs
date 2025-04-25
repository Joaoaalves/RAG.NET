namespace RAGNET.Domain.Entities.Jobs
{
    public class Job
    {
        public Guid JobId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; }

        public List<CallbackUrl> CallbackUrls { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}