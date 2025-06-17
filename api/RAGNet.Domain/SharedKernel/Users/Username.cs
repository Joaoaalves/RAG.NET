using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Users
{
    public class UserName : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        protected UserName() { } // For EF

        public UserName(string value)
        {
            CheckRule(new Rules.UserNameMustBeValidRule(value));
            Value = value;
        }

        public static implicit operator string(UserName userName) => userName.Value;
        public override string ToString() => Value;
        public override bool Equals(object? obj)
        {
            if (obj is UserName otherUserName)
            {
                return Value.Equals(otherUserName.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}