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

        public float[] ToArray() => [.. Vector];
        public int Size() => Vector.Count;
        public float At(int index) => Vector[index];
        public override string ToString() => $"[{string.Join(", ", Vector.Take(5))}...]";
    }
}