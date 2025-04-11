using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.ContentFilter;
using RAGNET.Application.Filters;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.ContentFilterUseCases;
using RAGNET.Domain.Entities;

namespace web.Controllers
{
    [Route("api/workflows")]
    [ApiController]
    public class ContentFilterController(
        ICreateContentFilterUseCase createContentFilterUseCase,
        IUpdateContentFilterUseCase updateContentFilterUseCase,
        IDeleteContentFilterUseCase deleteContentFilterUseCase,
        UserManager<User> userManager
    ) : ControllerBase
    {
        readonly UserManager<User> _userManager = userManager;

        readonly ICreateContentFilterUseCase _createContentFilterUseCase = createContentFilterUseCase;
        readonly IUpdateContentFilterUseCase _updateContentFilterUseCase = updateContentFilterUseCase;
        readonly IDeleteContentFilterUseCase _deleteContentFilterUseCase = deleteContentFilterUseCase;

        [HttpPost("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> EnableRSE([FromBody] RSECreationDTO dto, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");
                if (workflow.Filter != null && workflow.Filter.IsEnabled)
                {
                    return BadRequest("Relevant Segment Extraction already enabled!");
                }
                var filter = dto.ToFilter(workflow.Id, user.Id);
                filter.IsEnabled = true;
                var rse = await _createContentFilterUseCase.Execute(filter, workflow.Id, user.Id);

                return Ok(new { Message = "Relevant Segment Extraction enabled!", Filter = rse.ToDTO() });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPut("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> UpdateRSE([FromBody] RSECreationDTO dto, Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.Filter == null)
                    return BadRequest("Relevant Segment Extraction not enabled!");

                var filter = dto.ToFilter(workflow.Id, user.Id);

                if (dto.IsEnabled == null)
                {
                    filter.IsEnabled = workflow.Filter.IsEnabled;
                }

                var rseDto = await _updateContentFilterUseCase.Execute(workflow.Filter.Id, filter, user.Id);

                return Ok(new { Message = "Relevant Segment Extraction updated!", Filter = rseDto });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpDelete("{workflowId}/content-filter/rse")]
        [ServiceFilter(typeof(WebWorkflowFilter))]
        public async Task<IActionResult> DeleteRSE(Guid workflowId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflow = HttpContext.Items["Workflow"] as Workflow ?? throw new Exception("Workflow not found in context");

                if (workflow.Filter == null)
                    return BadRequest("Relevant Segment Extraction not enabled!");

                var rseDto = await _deleteContentFilterUseCase.Execute(workflow.Filter.Id, user.Id);

                return Ok(new { Message = "Relevant Segment Extraction deleted!", Filter = rseDto });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}