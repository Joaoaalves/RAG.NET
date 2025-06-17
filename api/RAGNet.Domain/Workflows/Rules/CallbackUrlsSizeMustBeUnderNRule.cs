using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Domain.Workflows.Rules
{
    public class CallbackUrlsSizeMustBeUnderNRule(IReadOnlyCollection<CallbackUrl> callbackUrls, int maxSize) : IBusinessRule
    {
        private readonly int _maxSize = maxSize;
        private readonly IReadOnlyCollection<CallbackUrl> _callbackUrls = callbackUrls;

        public string Message => $"The number of callback URLs must not exceed {_maxSize}.";

        public bool IsBroken() => _callbackUrls.Count > _maxSize;
    }
}