using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.Workflows
{
    public class WorkflowId : TypedIdValueBase
    {
        public WorkflowId(Guid value) : base(value)
        {

        }
        public WorkflowId() : base(Guid.NewGuid())
        {

        }
    }
}