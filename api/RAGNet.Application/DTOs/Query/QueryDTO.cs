namespace RAGNET.Application.DTOs.Query
{
    public class QueryDTO
    {
        public string Query { get; set; } = String.Empty;
        public int TopK { get; set; } = 5;
        public bool ParentChild { get; set; } = false;
        public bool NormalizeScore { get; set; } = false;
        public double? MinNormalizedScore { get; set; }
        public double? MinScore { get; set; }
    }
}