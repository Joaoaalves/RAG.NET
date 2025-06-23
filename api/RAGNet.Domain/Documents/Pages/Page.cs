using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Documents.Rules;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Pages
{
    public class Page : Entity
    {
        public List<Chunk> _chunks = [];
        public PageId Id { get; private init; } = default!;
        public Text Text { get; private set; } = default!;
        public DocumentId DocumentId { get; private set; } = default!;
        public Document Document { get; private set; } = null!;

        public IReadOnlyCollection<Chunk> Chunks => _chunks;

        // EF Core
        private Page() { }

        private Page(PageId id, Text text, DocumentId documentId)
        {
            Id = id;
            Text = text;
            DocumentId = documentId;
        }

        public static Page Create(Text text, DocumentId documentId, PageId? id = null)
        {
            CheckRule(new DocumentIdMustBeValid(documentId));

            return new Page(id ?? new PageId(), text, documentId);
        }
    }
}