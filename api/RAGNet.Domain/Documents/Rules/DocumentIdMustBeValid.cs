using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Rules
{
    public class DocumentIdMustBeValid(DocumentId documentId) : IBusinessRule
    {
        private readonly DocumentId _documentId = documentId;

        public string Message => "The document ID must be a valid GUID.";

        public bool IsBroken() => _documentId == new DocumentId(Guid.Empty);
    }
}