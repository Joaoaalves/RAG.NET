using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.URLs.Rules
{
    public class URLMustBeValidRule(string url) : IBusinessRule
    {
        private readonly string _url = url;

        public string Message => "The URL must be a valid format.";

        public bool IsBroken()
        {
            if (string.IsNullOrWhiteSpace(_url))
                return true;

            return !Uri.TryCreate(_url, UriKind.Absolute, out var uriResult) ||
                   (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps);
        }
    }
}