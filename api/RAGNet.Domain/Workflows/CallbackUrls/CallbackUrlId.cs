using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Workflows.CallbackUrls
{
    public class CallbackUrlId : TypedIdValueBase
    {
        public CallbackUrlId(Guid value) : base(value)
        {
        }
        public CallbackUrlId() : base(Guid.NewGuid())
        {
        }
    }
}