using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.URLs
{
    public class URL : ValueObject
    {
        public string Value { get; private set; } = string.Empty;

        // EF Core
        private URL() { }

        private URL(string url)
        {
            Value = url;
        }

        public static URL Create(string url)
        {
            CheckRule(new Rules.URLMustBeValidRule(url));

            return new URL(url);
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(URL url)
        {
            return url.Value;
        }
        public void SetValue(string url)
        {
            CheckRule(new Rules.URLMustBeValidRule(url));

            Value = url;
        }

        public override bool Equals(object? obj)
        {
            if (obj is URL otherUrl)
            {
                return Value.Equals(otherUrl.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
