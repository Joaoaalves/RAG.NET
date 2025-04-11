using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Interfaces;

namespace RAGNET.Domain.Entities
{
    public class Chunker : IUserOwned
    {
        [Key]
        public Guid Id { get; set; }
        public ChunkerStrategy StrategyType { get; set; }

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;

        public ICollection<ChunkerMeta> Metas { get; set; } = [];
    }

    public class ChunkerMeta
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Chunker")]
        public Guid ChunkerId { get; set; }
        public Chunker Chunker { get; set; } = null!;
        public string Key { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;
    }
}