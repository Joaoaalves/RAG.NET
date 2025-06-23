using RAGNET.Application.Configuration.ExecutionContext;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Configuration.Commands
{
    public abstract class WorkflowAndUserAwareCommand<TResult> : IWorkflowAware, IUserAware, ICommand<TResult>
    {
        public Guid Id => Guid.NewGuid();
        public Workflow Workflow { get; private set; } = default!;
        public User User { get; private set; } = default!;

        public void InjectUser(User user)
        {
            User = user;
        }

        public void InjectWorkflow(Workflow workflow)
        {
            Workflow = workflow;
        }
    }
}