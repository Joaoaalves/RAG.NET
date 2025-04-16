namespace RAGNET.Domain.Entities
{
    public class DocumentExtractResult
    {
        public string DocumentTitle { get; set; } = String.Empty;
        public List<string> Pages { get; set; } = [];
    }

}