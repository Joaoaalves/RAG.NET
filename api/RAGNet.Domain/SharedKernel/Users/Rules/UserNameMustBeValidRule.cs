using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Users.Rules
{
    public partial class UserNameMustBeValidRule(string userName) : IBusinessRule
    {
        private readonly string _userName = userName;

        public string Message => "The username must be at least 3 characters long and can only contain letters, numbers, and underscores.";

        public bool IsBroken() => string.IsNullOrWhiteSpace(_userName) || _userName.Length < 3 || !UserNameRegex().IsMatch(_userName);
        [System.Text.RegularExpressions.GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        private static partial System.Text.RegularExpressions.Regex UserNameRegex();
    }
}