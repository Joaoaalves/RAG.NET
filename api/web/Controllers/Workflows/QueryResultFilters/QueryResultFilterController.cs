using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.DTOs.QueryResultFilter;
using RAGNET.Application.Filters;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.QueryResultFilterUseCases;
using RAGNET.Application.QueryResultFilters;

using RAGNET.Domain.Users;
using RAGNET.Domain.Workflows;


namespace web.Controllers.Workflows.QueryResultFilters
{
    [Route("api/workflows")]
    [ApiController]
    public class QueryResultFilterController(
        ICreateQueryResultFilterUseCase createQueryResultFilterUseCase,
        IUpdateQueryResultFilterUseCase updateQueryResultFilterUseCase,
        IDeleteQueryResultFilterUseCase deleteQueryResultFilterUseCase,
        UserManager<User> userManager
    ) : ControllerBase
    {
        readonly UserManager<User> _userManager = userManager;

        readonly ICreateQueryResultFilterUseCase _createQueryResultFilterUseCase = createQueryResultFilterUseCase;
        readonly IUpdateQueryResultFilterUseCase _updateQueryResultFilterUseCase = updateQueryResultFilterUseCase;
        readonly IDeleteQueryResultFilterUseCase _deleteQueryResultFilterUseCase = deleteQueryResultFilterUseCase;

        [HttpPost("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableRSE([FromBody] RSECreationDTO dto, [FromRoute] Guid workflowId)
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
                var filter = dto.ToFilter(workflow.Id, user.Id);

                var rse = await _createQueryResultFilterUseCase.Execute(filter, workflow.Id, user.Id);

                return Ok(new { Message = "Relevant Segment Extraction enabled!", QueryResultFilter = rse.ToDTO() });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> UpdateRSE([FromBody] RSECreationDTO dto, [FromRoute] Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.QueryResultFilter == null)
                    return BadRequest("Relevant Segment Extraction not enabled!");

                var filter = dto.ToFilter(workflow.Id, user.Id);

                if (dto.IsEnabled == null)
                {
                    filter.SetEnableState(workflow.QueryResultFilter.IsEnabled);
                }

                var rseDto = await _updateQueryResultFilterUseCase.Execute(workflow.QueryResultFilter.Id, filter, user.Id);

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

                var rseDto = await _deleteQueryResultFilterUseCase.Execute(workflow.QueryResultFilter.Id, user.Id);

                return Ok(new { Message = "Relevant Segment Extraction deleted!", QueryResultFilter = rseDto });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}