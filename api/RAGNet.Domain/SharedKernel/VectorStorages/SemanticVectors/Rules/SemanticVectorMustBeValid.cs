using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.VectorStorages.SemanticVectors.Rules
{
    public class SemanticVectorMustBeValid(float[] vector) : IBusinessRule
    {
        private readonly float[] _vector = vector;

        public string Message => "The vector must not be null and must have a valid length.";

        public bool IsBroken() => _vector == null || _vector.Length == 0;
    }
}