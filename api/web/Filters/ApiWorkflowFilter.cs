using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RAGNET.Domain.SharedKernel.Plans.Policies;
using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;

namespace web.Filters
{
    public class ApiWorkflowFilter(IWorkflowRepository workflowRepository, IUserRepository userRepository, SubscriptionPolicy subscriptionPolicy) : IAsyncActionFilter
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly SubscriptionPolicy _subscriptionPolicy = subscriptionPolicy;
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiKey = context.HttpContext.Request.Headers["x-api-key"].ToString();

            if (string.IsNullOrEmpty(apiKey))
            {
                context.Result = new BadRequestObjectResult("API key is missing.");
                return;
            }

            var workflow = await _workflowRepository.GetByApiKey(apiKey);

            if (workflow == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API key.");
                return;
            }

            var isAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
            {
                var user = await _userRepository.GetByIdAsync(workflow.UserId);

                if (user is null || !_subscriptionPolicy.AllowsApiUsage(user))
                {
                    context.Result = new UnauthorizedObjectResult("Your plan does not allow API usage.");
                    return;
                }

                context.HttpContext.Items["User"] = user;
            }

            context.HttpContext.Items["Workflow"] = workflow;

            await next();
        }
    }
}