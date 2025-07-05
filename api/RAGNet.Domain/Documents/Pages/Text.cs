using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Pages
{
    public class Text : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        protected Text() { } // For EF

        public Text(string value = "")
        {
            CheckRule(new Rules.TextMustNotBeEmptyRule(value));
            value = value.Trim();

            Value = value;
        }

        public override string ToString() => Value;

        public void SetValue(string value)
        {
            value = value.Trim();

            CheckRule(new Rules.TextMustNotBeEmptyRule(value));

            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Text otherText)
            {
                return Value.Equals(otherText.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}