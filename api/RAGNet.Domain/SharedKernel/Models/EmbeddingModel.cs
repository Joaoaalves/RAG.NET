using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Models
{
    public class EmbeddingModel : ValueObject
    {
        public string Label { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
        public int Speed { get; init; }
        public float Price { get; init; }
        public int VectorSize { get; init; }
        public int MaxContext { get; init; }

    }
}
