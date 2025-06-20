using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// Domain
using RAGNET.Domain.Users;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Workflows;

using RAGNET.Application.Filters;
using RAGNET.Application.QueryEnhancers.CreateQueryEnhancer;
using RAGNET.Application.QueryEnhancers;
using RAGNET.Application.QueryEnhancers.UpdateQueryEnhancer;
using RAGNET.Application.QueryEnhancers.DeleteQueryEnhancer;

using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Workflows.QueryEnhancers
{
    [Route("api/workflows")]
    [ApiController]
    public class QueryEnhancerController(
        CommandsExecutor commandsExecutor,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost("{workflowId}/query-enhancer/auto-query")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableAutoQuery([FromBody] CreateAutoQueryRequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.QueryEnhancers.Any(qe => qe.Type == QueryEnhancerStrategy.AUTO_QUERY))
                    return BadRequest("Auto Query already enabled!");

                var command = new CreateQueryEnhancerCommand(
                    request.ToQueryEnhancer(workflow.Id, user.Id)
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Auto Query enabled!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{workflowId}/query-enhancer/auto-query")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> UpdateAutoQuery([FromBody] UpdateAutoQueryRequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                var qe = workflow.QueryEnhancers.FirstOrDefault(qe => qe.Type == QueryEnhancerStrategy.AUTO_QUERY);

                if (qe == null)
                    return BadRequest("Auto Query not enabled!");

                var command = new UpdateQueryEnhancerCommand(
                    user.Id,
                    qe.Id,
                    QueryEnhancerStrategy.AUTO_QUERY,
                    request.MaxQueries,
                    request.IsEnabled,
                    request.Guidance
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Auto Query updated!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{workflowId}/query-enhancer/auto-query")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> DisableAutoQuery([FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                var qe = workflow.QueryEnhancers.FirstOrDefault(qe => qe.Type == QueryEnhancerStrategy.AUTO_QUERY);

                if (qe == null)
                    return BadRequest("Auto Query not enabled!");

                var command = new DeleteQueryEnhancerCommand(
                    user.Id,
                    qe.Id
                );
                var result = await _commandsExecutor.Execute(command);

                if (result)
                    return BadRequest("Something went wrong, Auto Query was not disabled!");

                return Ok(new { Message = "Auto Query disabled!" });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPost("{workflowId}/query-enhancer/hyde")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableHyde([FromBody] CreateHyDERequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.QueryEnhancers.Any(qe => qe.Type == QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING))
                    return BadRequest("HyDE already enabled!");

                var command = new CreateQueryEnhancerCommand(
                    request.ToQueryEnhancer(workflow.Id, user.Id)
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Hyde enabled!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{workflowId}/query-enhancer/hyde")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> UpdateHyde([FromBody] UpdateHyDERequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                var qe = workflow.QueryEnhancers.FirstOrDefault(qe => qe.Type == QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING);

                if (qe == null)
                    return BadRequest("HyDE not enabled!");

                var command = new UpdateQueryEnhancerCommand(
                    user.Id,
                    qe.Id,
                    QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING,
                    request.MaxQueries,
                    request.IsEnabled
                );

                var queryEnhancer = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Hyde updated!", queryEnhancer });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{workflowId}/query-enhancer/hyde")]
        [Authorize]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> DisableHyde([FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                var qe = workflow.QueryEnhancers.FirstOrDefault(qe => qe.Type == QueryEnhancerStrategy.HYPOTHETICAL_DOCUMENT_EMBEDDING);

                if (qe == null)
                    return BadRequest("HyDE not enabled!");

                var command = new DeleteQueryEnhancerCommand(
                    user.Id,
                    qe.Id
                );
                var result = await _commandsExecutor.Execute(command);


                if (result)
                    return BadRequest("Something went wrong, HyDE was not disabled!");

                return Ok(new { Message = "Hyde disabled!" });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}