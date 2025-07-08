using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Configuration.Commands
{
    public class WorkflowAwareCommand<TResult> : IWorkflowAware, IUserAware, ICommand<TResult>
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Workflow Workflow { get; private set; } = default!;
        public User User { get; private set; } = default!;

        public void InjectWorkflow(Workflow workflow)
        {
            Workflow = workflow;
        }

        public void InjectUser(User user)
        {
            User = user;
        }
    }
}