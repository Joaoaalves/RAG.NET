using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Pages.Chunks
{
    public class Score : ValueObject
    {
        public double Value { get; private set; } = 0.0;

        public Score(double value)
        {
            CheckRule(new Rules.ScoreMustBeNormalized(value));
            Value = value;
        }

        public override string ToString() => Value.ToString("0.0000");
    }
}