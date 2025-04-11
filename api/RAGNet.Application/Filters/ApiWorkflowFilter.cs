using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.Filters
{
    public class ApiWorkflowFilter(IWorkflowRepository workflowRepository) : IAsyncActionFilter
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiKey = context.HttpContext.Request.Headers["x-api-key"].ToString();

            if (string.IsNullOrEmpty(apiKey))
            {
                context.Result = new BadRequestObjectResult("API key is missing.");
                return;
            }

            var workflow = await _workflowRepository.GetWithRelationsByApiKey(apiKey);

            if (workflow == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API key.");
                return;
            }

            context.HttpContext.Items["Workflow"] = workflow;

            await next();
        }
    }
}