using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.VectorStorages.Scores.Rules
{
    public class ScoreMustBeNormalized(double score) : IBusinessRule
    {
        private readonly double _score = score;

        public string Message => "The score must be normalized between 0 and 1.";

        public bool IsBroken() => _score < 0 || _score > 1;
    }
}