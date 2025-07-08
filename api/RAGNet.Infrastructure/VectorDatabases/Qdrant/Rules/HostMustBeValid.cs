using System.Text.RegularExpressions;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Infrastructure.VectorDatabases.Qdrant.Rules
{
    public class HostMustBeValidRule(string host, string pattern) : IBusinessRule
    {
        public string Message => "The host is not valid. It must be an URL";
        public string Host { get; } = host;
        public string Pattern { get; } = pattern;
        public bool IsBroken()
        {
            return !Regex.IsMatch(Host, Pattern);
        }
    }
}