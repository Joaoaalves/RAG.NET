using RAGNET.Domain.Documents.Pages;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.Documents
{
    public class Document : Entity, IAggregateRoot
    {
        public readonly List<Page> _pages = [];

        public Guid Id { get; private init; } = default!;
        public Text Title { get; private set; } = default!;
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;

        public IReadOnlyCollection<Page> Pages => _pages.AsReadOnly();

        // EF Core ctor

        private Document() { }

        private Document(Guid id, Text title, Guid workflowId, IEnumerable<Page>? pages = null)
        {
            Id = id;
            Title = title;
            WorkflowId = workflowId;

            if (pages != null)
                _pages.AddRange(pages);
        }

        public static Document Create(Text title, Guid workflowId, Guid? id = null, IEnumerable<Page>? pages = null)
        {
            return new Document(id ?? Guid.NewGuid(), title, workflowId, pages);
        }

        public void AddPage(Page page)
        {
            ArgumentNullException.ThrowIfNull(page);

            _pages.Add(page);
        }

        public void SetTitle(Text title)
        {
            Title = title;
        }
    }
}