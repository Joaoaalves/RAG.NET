using System.ComponentModel.DataAnnotations.Schema;

namespace RAGNET.Domain.Entities
{
    public class CallbackUrl
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = String.Empty;

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
    }
}