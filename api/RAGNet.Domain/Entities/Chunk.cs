using System.ComponentModel.DataAnnotations.Schema;

namespace RAGNET.Domain.Entities
{
    public class Chunk
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public string VectorId { get; set; } = String.Empty;

        [ForeignKey("Page")]
        public Guid PageId { get; set; }
        public Page Page { get; set; } = null!;

        [NotMapped]
        public float[][] Embedding { get; set; } = [];
        [NotMapped]
        public double Score { get; set; }
    }
}