namespace RAGNET.Domain.Entities
{
    public class EmbeddingModel
    {
        public string Label { get; set; } = String.Empty;
        public string Value { get; set; } = String.Empty;
        public int Speed { get; set; } = 0;
        public float Price { get; set; } = 0;
        public int VectorSize { get; set; } = 0;
        public int MaxContext { get; set; } = 0;
    }
}