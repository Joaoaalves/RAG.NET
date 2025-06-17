using System.Text.RegularExpressions;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Users.Rules
{
    public partial class NameMustBeValidRule(string value) : IBusinessRule
    {
        private readonly string _value = value;

        public bool IsBroken() =>
            string.IsNullOrWhiteSpace(_value) ||
            _value.Length < 3 ||
            !NameRegex().IsMatch(_value);

        public string Message => "The name must be at least 3 characters long and contain only letters, and spaces.";

        [GeneratedRegex(@"^[a-zA-Z0]+$")]
        private static partial Regex NameRegex();
    }
}