using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.Query;
using RAGNET.Application.Filters;
using RAGNET.Application.UseCases;
using RAGNET.Domain.Entities;

namespace web.Controllers
{
    [Route("/api/")]
    [ApiController]
    public class QueryController(
        IProcessQueryUseCase processQueryUseCase) : ControllerBase
    {
        private readonly IProcessQueryUseCase _processsQueryUseCase = processQueryUseCase;

        [HttpPost("query")]
        [ServiceFilter(typeof(ApiWorkflowFilter))]
        public async Task<IActionResult> Query([FromBody] QueryDTO queryDTO)
        {
            try
            {
                var workflow = HttpContext.Items["Workflow"] as Workflow
                    ?? throw new Exception("Workflow not found in context.");

                var result = await _processsQueryUseCase.Execute(workflow, queryDTO);

                return Ok(new { Chunks = result });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}