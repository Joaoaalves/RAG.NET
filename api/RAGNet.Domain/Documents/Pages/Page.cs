using RAGNET.Domain.Documents.Pages.Chunks;
using RAGNET.Domain.Documents.Rules;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Pages
{
    public class Page : Entity
    {
        public List<Chunk> _chunks = [];
        public Guid Id { get; private init; } = default!;
        public Text Text { get; private set; } = default!;
        public Guid DocumentId { get; private set; } = default!;
        public Document Document { get; private set; } = null!;

        public IReadOnlyCollection<Chunk> Chunks => _chunks;

        // EF Core
        private Page() { }

        private Page(Guid id, Text text, Guid documentId)
        {
            Id = id;
            Text = text;
            DocumentId = documentId;
        }

        public static Page Create(Text text, Guid documentId, Guid? id = null)
        {
            CheckRule(new DocumentIdMustBeValid(documentId));

            return new Page(id ?? Guid.NewGuid(), text, documentId);
        }
    }
}