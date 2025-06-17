using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Users
{
    public class Name : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        protected Name()
        { } // For EF
        public Name(string value)
        {
            value = value.Trim();
            CheckRule(new Rules.NameMustBeValidRule(value));
            Value = value;
        }

        public override string ToString() => Value;
        public void SetValue(string value)
        {
            value = value.Trim();
            CheckRule(new Rules.NameMustBeValidRule(value));
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Name otherName)
            {
                return Value.Equals(otherName.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}