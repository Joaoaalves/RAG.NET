using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Interfaces;

namespace RAGNET.Domain.Entities
{
    public class Workflow : IUserOwned
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = String.Empty;
        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
        public string ApiKey { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Related configurations
        public ICollection<Chunker> Chunkers { get; set; } = null!;
        public ICollection<QueryEnhancer> QueryEnhancers { get; set; } = null!;
        public ICollection<Filter> Filters { get; set; } = null!;
        public ICollection<Ranker> Rankers { get; set; } = null!;
    }
}