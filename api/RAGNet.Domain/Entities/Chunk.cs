using System.ComponentModel.DataAnnotations.Schema;

namespace RAGNET.Domain.Entities
{
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