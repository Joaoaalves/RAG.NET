namespace RAGNET.Domain.Entities.Jobs
{
    public class Job<TContext> where TContext : JobProcessingContext
    {
        public Guid JobId { get; set; } = Guid.NewGuid();
        public string ApiKey { get; set; } = String.Empty;
        public string UserId { get; set; } = String.Empty;
        public List<string> CallbackUrls { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public TContext Context { get; set; } = null!;
    }
}