using Microsoft.AspNetCore.Mvc;

using web.Filters;

using RAGNET.Infrastructure.Processing;
using RAGNET.Application.QueryResultFilters.Commands.CreateQueryResultFilter;
using RAGNET.Application.QueryResultFilters.Commands.UpdateQueryResultFilter;
using RAGNET.Application.QueryResultFilters.Commands.DeleteQueryResultFilter;
using RAGNET.Domain.QueryResultFilters;


namespace web.Controllers.Workflows.QueryResultFilters
{
    [Route("api/workflows")]
    [ApiController]
    public class QueryResultFilterController(
        CommandsExecutor commandsExecutor
    ) : ControllerBase
    {
        readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        [HttpPost("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableRSE([FromBody] CreateQueryResultFilterCommand command, [FromRoute] Guid workflowId)
        {
            try
            {
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
                var command = new UpdateQueryResultFilterCommand(
                    QueryResultFilterStrategy.RELEVANT_SEGMENT_EXTRACTION,
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
                var command = new DeleteQueryResultFilterCommand(
                    QueryResultFilterStrategy.RELEVANT_SEGMENT_EXTRACTION
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