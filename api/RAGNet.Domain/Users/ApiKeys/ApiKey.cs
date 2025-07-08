using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Users.ApiKeys
{
    public class ApiKey : ValueObject
    {
        public string Value { get; private set; } = String.Empty;

        [IgnoreMember]
        public string Suffix => Value.Length >= 4 ? Value[^4..] : Value;

        protected ApiKey() { } // For EF

        public ApiKey(string value)
        {
            CheckRule(new Rules.ApiKeyMustBeValidRule(value));
            Value = value;
        }

        public override string ToString() => $"****{Suffix}";
    }
}