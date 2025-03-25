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

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;
        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
        public ChunkerStrategy StrategyType { get; set; }

        public ICollection<EmbeddingProviderConfig> EmbeddingProvider { get; set; } = null!;

        public ICollection<ChunkerMeta> Metas { get; set; } = [];
    }

    public class ChunkerMeta
    {
        [Key]
        public Guid Ìd { get; set; }

        [ForeignKey("Chunker")]
        public Guid ChunkerId { get; set; }
        public Chunker Chunker { get; set; } = null!;
        public string Key { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;
    }

    public class EmbeddingProviderConfig
    {
        [Key]
        public Guid Id { get; set; }
        public EmbeddingProviderEnum Provider { get; set; }
        public string ApiKey { get; set; } = String.Empty;

        [ForeignKey("Chunker")]
        public Guid ChunkerId { get; set; }
        public Chunker Chunker { get; set; } = null!;
    }

    public class Chunk
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = String.Empty;

        // Coming from VectorDB
        public string DocumentId { get; set; } = String.Empty;

        [NotMapped]
        public float[][] Embedding { get; set; } = [];
        [NotMapped]
        public double Score { get; set; }
    }

}