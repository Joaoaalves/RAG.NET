using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RAGNET.Domain.Entities;

namespace RAGNET.Domain.Documents
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = String.Empty!;
        public ICollection<Page> Pages { get; set; } = [];

        [ForeignKey("Workflow")]
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;
    }
}