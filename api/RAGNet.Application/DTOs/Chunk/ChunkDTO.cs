namespace RAGNET.Application.DTOs.Chunk
{
    public class ChunkDTO
    {
        public string Text { get; set; } = String.Empty;
        public string VectorId { get; set; } = String.Empty;
        public Double Score { get; set; }
    }
}