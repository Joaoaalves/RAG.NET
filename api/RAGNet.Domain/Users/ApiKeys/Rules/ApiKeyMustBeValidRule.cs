using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Users.ApiKeys.Rules
{
    public class ApiKeyMustBeValidRule(string apiKey) : IBusinessRule
    {
        private readonly string _apiKey = apiKey;

        public string Message => "The API key must be at least 10 characters long.";

        public bool IsBroken() => string.IsNullOrWhiteSpace(_apiKey) || _apiKey.Length < 10;

    }
}