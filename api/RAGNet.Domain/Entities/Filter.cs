using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Interfaces;

namespace RAGNET.Domain.Entities
{
    public class Filter : IUserOwned
    {
        public Guid Id { get; set; }
        public FilterStrategy Strategy { get; set; }

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;
        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;
        public ICollection<FilterMeta> Metas { get; set; } = [];
    }

    public class FilterMeta
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Filter")]
        public Guid FilterId { get; set; }
        public Filter Filter { get; set; } = null!;

        public string Key { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;
    }
}