using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAGNET.Domain.Entities
{
    public class Page
    {
        [Key]
        public Guid Id { get; set; }
        public ICollection<Chunk> Chunks { get; set; } = [];

        [ForeignKey("Document")]
        public Guid DocumentId { get; set; }
        public Document Document { get; set; } = null!;
    }
}