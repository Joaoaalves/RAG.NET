using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Domain.Workflows.Rules
{
    public class CallbackUrlMustBeUniqueForWorkflow(string callbackUrl, IReadOnlyCollection<CallbackUrl> callbackUrls) : IBusinessRule
    {
        private readonly string _callbackUrl = callbackUrl;
        private readonly IReadOnlyCollection<CallbackUrl> _callbackUrls = callbackUrls;

        public string Message => "The callback URL must be unique for the workflow.";

        public bool IsBroken() => _callbackUrls.Any(c => c.Url.Equals(_callbackUrl));
    }
}