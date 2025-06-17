using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Rules
{
    public class DocumentIdMustBeValid(Guid documentId) : IBusinessRule
    {
        private readonly Guid _documentId = documentId;

        public string Message => "The document ID must be a valid GUID.";

        public bool IsBroken() => _documentId == Guid.Empty;
    }
}