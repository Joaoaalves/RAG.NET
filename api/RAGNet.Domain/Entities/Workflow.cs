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
        public string Description { get; set; } = String.Empty;
        public int DocumentsCount { get; set; } = 0;
        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
        public string ApiKey { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CollectionId { get; set; }

        // Related configurations
        public Chunker? Chunker { get; set; }
        public ICollection<Document> Documents { get; set; } = [];
        public ICollection<QueryEnhancer> QueryEnhancers { get; set; } = [];
        public ICollection<Filter> Filters { get; set; } = [];
        public ICollection<Ranker> Rankers { get; set; } = [];
        public ConversationProviderConfig? ConversationProviderConfig { get; set; }
        public EmbeddingProviderConfig? EmbeddingProviderConfig { get; set; }
    }
}