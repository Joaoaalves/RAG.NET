using RAGNET.Application.Configuration.ExecutionContext;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Configuration.Commands
{
    public class WorkflowAwareCommand<TResult> : IWorkflowAware, ICommand<TResult>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Workflow Workflow { get; private set; } = default!;

        public void InjectWorkflow(Workflow workflow)
        {
            Workflow = workflow;
        }
    }
}