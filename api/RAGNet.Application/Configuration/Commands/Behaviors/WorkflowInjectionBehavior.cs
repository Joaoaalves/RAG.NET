using Microsoft.AspNetCore.Http;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Configuration.Commands.Behaviors
{
    public class WorkflowInjectionBehavior<TCommand, TResult>(
        IHttpContextAccessor httpContextAccessor
    ) : ICommandPipelineBehavior<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<TResult> Handle(
            TCommand command,
            Func<TCommand, Task<TResult>> next,
            CancellationToken cancellationToken)
        {
            if (command is IWorkflowAware workflowAware)
            {
                if (_httpContextAccessor.HttpContext?.Items["Workflow"] is not Workflow workflow)
                    throw new Exception("Workflow not found");

                workflowAware.InjectWorkflow(workflow);
            }

            return await next(command);
        }
    }

}