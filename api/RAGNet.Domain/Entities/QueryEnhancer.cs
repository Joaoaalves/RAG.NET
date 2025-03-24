using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{
    public class QueryEnhancer
    {
        private string Id { get; set; } = String.Empty;
        private QueryEnhancerTypeEnum Type { get; set; }
        private string Prompt { get; set; } = String.Empty;
        private int MaxQueries { get; set; }
    }
}