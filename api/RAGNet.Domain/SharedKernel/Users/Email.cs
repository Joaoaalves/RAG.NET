using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Users
{
    public class Email : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        protected Email() { } // For EF

        public Email(string value)
        {
            CheckRule(new Rules.EmailMustBeValidRule(value));
            Value = value;
        }

        public override string ToString() => Value;
        public static implicit operator string(Email email)
        {
            return email.Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Email otherEmail)
            {
                return Value.Equals(otherEmail.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}