using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RAGNET.Domain.Repositories;
using System.Security.Claims;

namespace RAGNET.Application.Filters
{
    public class WebWorkflowFilter(IWorkflowRepository workflowRepository) : IAsyncActionFilter
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedObjectResult("User ID not found in token.");
                return;
            }

            if (!context.ActionArguments.TryGetValue("workflowId", out var workflowIdObj) || workflowIdObj is not Guid workflowId)
            {
                context.Result = new BadRequestObjectResult("Missing or invalid workflowId.");
                return;
            }

            var workflow = await _workflowRepository.GetWithRelationsAsync(workflowId, userId);

            if (workflow == null)
            {
                context.Result = new NotFoundObjectResult("Workflow not found or does not belong to user.");
                return;
            }

            context.HttpContext.Items["Workflow"] = workflow;
            await next();
        }
    }
}