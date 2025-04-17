using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{

    public class EmbeddingProviderConfig
    {
        [Key]
        public Guid Id { get; set; }
        public EmbeddingProviderEnum Provider { get; set; }
        public string Model { get; set; } = String.Empty;
        public int VectorSize { get; set; }

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;

    }
}