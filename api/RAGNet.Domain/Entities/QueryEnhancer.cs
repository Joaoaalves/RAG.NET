using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{
    public class QueryEnhancer
    {
        public Guid Id { get; set; }
        public QueryEnhancerStrategy Type { get; set; }

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;
        public string Prompt { get; set; } = String.Empty;
        public int MaxQueries { get; set; }
        public ICollection<QueryEnhancerMeta> Metas { get; set; } = [];
    }

    public class QueryEnhancerMeta
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("QueryEnhancer")]
        public Guid QueryEnhancerId { get; set; }
        public QueryEnhancer QueryEnhancer { get; set; } = null!;

        public string Key { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;
    }
}