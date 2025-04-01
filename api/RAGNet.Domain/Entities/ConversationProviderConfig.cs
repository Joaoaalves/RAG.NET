using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{

    public class ConversationProviderConfig
    {
        [Key]
        public Guid Id { get; set; }
        public ConversationProviderEnum Provider { get; set; }
        public string ApiKey { get; set; } = String.Empty;
        public string Model { get; set; } = String.Empty;

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;
    }
}