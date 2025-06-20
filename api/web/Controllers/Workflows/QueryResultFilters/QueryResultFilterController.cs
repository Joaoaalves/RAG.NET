using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.Filters;
using RAGNET.Application.QueryResultFilters;

using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;
using RAGNET.Infrastructure.Processing;
using RAGNET.Application.QueryResultFilters.CreateQueryResultFilter;
using RAGNET.Application.QueryResultFilters.UpdateQueryResultFilter;
using RAGNET.Application.QueryResultFilters.DeleteQueryResultFilter;


namespace web.Controllers.Workflows.QueryResultFilters
{
    [Route("api/workflows")]
    [ApiController]
    public class QueryResultFilterController(
        CommandsExecutor commandsExecutor,
        UserManager<User> userManager
    ) : ControllerBase
    {
        readonly UserManager<User> _userManager = userManager;
        readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        [HttpPost("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableRSE([FromBody] RSECreationRequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.QueryResultFilter != null && workflow.QueryResultFilter.IsEnabled)
                {
                    return BadRequest("Relevant Segment Extraction already enabled!");
                }

                var filter = request.ToFilter(workflow.Id, user.Id);
                var command = new CreateQueryResultFilterCommand(
                    workflow.Id,
                    user.Id,
                    filter
                );

                var rse = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Relevant Segment Extraction enabled!", Filter = rse });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> UpdateRSE([FromBody] QueryResultFilterUpdateRequest request, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.QueryResultFilter == null)
                    return BadRequest("Relevant Segment Extraction not enabled!");

                var command = new UpdateQueryResultFilterCommand(
                    workflow.QueryResultFilter.Id,
                    user.Id,
                    request
                );

                var rseDto = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Relevant Segment Extraction updated!", QueryResultFilter = rseDto });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> DeleteRSE([FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.QueryResultFilter == null)
                    return BadRequest("Relevant Segment Extraction not enabled!");

                var command = new DeleteQueryResultFilterCommand(
                    workflow.QueryResultFilter.Id,
                    user.Id
                );

                var rseDto = await _commandsExecutor.Execute(command);

                return Ok(new { Message = "Relevant Segment Extraction deleted!", QueryResultFilter = rseDto });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}