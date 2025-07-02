using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Users.Subscriptions
{
    public class SubscriptionId : TypedIdValueBase
    {
        public SubscriptionId(Guid value) : base(value) { }
        public SubscriptionId() : base(Guid.NewGuid()) { }

    }
}