using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Metas
{
    public class Meta : ValueObject
    {
        public string Key { get; private set; } = string.Empty;
        public string Value { get; private set; } = string.Empty;

        private Meta() { } // EF Core

        public Meta(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public void UpdateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be empty.");
            Key = key;
        }

        public void UpdateValue(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }
    }
}