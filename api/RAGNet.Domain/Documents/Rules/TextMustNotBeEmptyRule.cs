using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Documents.Rules
{
    public class TextMustNotBeEmptyRule(string text) : IBusinessRule
    {
        private readonly string _text = text;

        public string Message => "The text must not be empty.";

        public bool IsBroken() => string.IsNullOrWhiteSpace(_text);
    }
}