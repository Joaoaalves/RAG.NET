using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Pages.Chunks
{
    public class SemanticVector : ValueObject
    {
        public IReadOnlyList<float> Vector { get; } = [];

        public SemanticVector(float[] vector)
        {
            CheckRule(new Rules.SemanticVectorMustBeValid(vector));
            Vector = vector.ToList().AsReadOnly();
        }

        public override string ToString() => $"[{string.Join(", ", Vector.Take(5))}...]";
    }
}