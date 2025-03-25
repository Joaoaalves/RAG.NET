using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RAGNET.Domain.Repositories;

namespace RAGNET.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiKeyCheckAttribute(string headerName = "x-api-key") : Attribute, IAsyncActionFilter
    {
        private readonly string _headerName = headerName;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_headerName, out var potentialApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API key não fornecida.");
                return;
            }

            if (!context.ActionArguments.TryGetValue("workflow_id", out var workflowIdObj) ||
                workflowIdObj is not Guid workflowId)
            {
                context.Result = new BadRequestObjectResult("Workflow id inválido.");
                return;
            }

            if (context.HttpContext.RequestServices.GetService(typeof(IWorkflowRepository)) is not IWorkflowRepository workflowRepository)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                return;
            }

            await next();
        }
    }
}