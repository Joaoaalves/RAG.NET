using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Enums;
using RAGNET.Domain.Interfaces;

namespace RAGNET.Domain.Entities
{
    public class Ranker : IUserOwned
    {
        public Guid Id { get; set; }

        public RankerStrategy Strategy { get; set; }

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;
        [ForeignKey("User")]
        public string UserId { get; set; } = String.Empty;

        public ICollection<RankerMeta> Metas { get; set; } = [];
    }
    public class RankerMeta
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Ranker")]
        public Guid RankerId { get; set; }
        public Ranker Ranker { get; set; } = null!;

        public string Key { get; set; } = String.Empty;

        public string Value { get; set; } = String.Empty;
    }

}