using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.SeedWork
{
    public interface IWorkflowAware
    {
        void InjectWorkflow(Workflow workflow);
    }
}