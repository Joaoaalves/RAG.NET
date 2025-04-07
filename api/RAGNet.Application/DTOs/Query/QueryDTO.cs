namespace RAGNET.Application.DTOs.Query
{
    public class QueryDTO
    {
        public string Query { get; set; } = String.Empty;
        public int TopK { get; set; } = 5;
    }
}