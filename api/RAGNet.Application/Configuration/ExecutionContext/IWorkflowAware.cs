using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Configuration.ExecutionContext
{
    public interface IWorkflowAware
    {
        void InjectWorkflow(Workflow workflow);
    }
}