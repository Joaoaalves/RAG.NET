using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using RAGNET.Domain.Entities;
using RAGNET.Domain.Exceptions;

using RAGNET.Application.DTOs.Workflow;
using RAGNET.Application.UseCases.WorkflowUseCases;
using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Application.Filters;

namespace web.Controllers
{
    [Route("api/workflows")]
    [ApiController]
    public class WorkflowController(
        IGetUserWorkflowsUseCase getUserWorkflowsUseCase,
        ICreateWorkflowUseCase createWorkflowUseCase,
        IDeleteWorkflowUseCase deleteWorkflowUseCase,
        IGetWorkflowUseCase getWorkflowUseCase,
        IProcessEmbeddingUseCase processEmbeddingUseCase,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly IGetUserWorkflowsUseCase _getUserWorkflowsUseCase = getUserWorkflowsUseCase;
        private readonly ICreateWorkflowUseCase _createWorkflowUseCase = createWorkflowUseCase;
        private readonly IDeleteWorkflowUseCase _deleteWorkflowUseCase = deleteWorkflowUseCase;
        private readonly IGetWorkflowUseCase _getWorkflowUseCase = getWorkflowUseCase;
        private readonly IProcessEmbeddingUseCase _processEmbeddingUseCase = processEmbeddingUseCase;
        private readonly UserManager<User> _userManager = userManager;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowCreationDTO dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var workflowId = await _createWorkflowUseCase.Execute(dto, user);
                return Ok(new { Message = "Workflow created!", WorkflowId = workflowId });
            }
            catch (InvalidEmbeddingModelException exc)
            {
                return BadRequest(exc.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWorkflows()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var workflows = await _getUserWorkflowsUseCase.Execute(user.Id);

            return Ok(new { Workflows = workflows });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetWorkflow(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            try
            {
                var workflowDetails = await _getWorkflowUseCase.Execute(id, user.Id);
                return Ok(workflowDetails);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkflow(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            try
            {
                var workflow = await _deleteWorkflowUseCase.Execute(id, user.Id);
                return Ok(workflow);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("embedding")]
        [Consumes("multipart/form-data")]
        [ServiceFilter(typeof(ApiWorkflowFilter))]
        public async Task ProcessEmbedding(IFormFile file, [FromQuery] bool stream = false)
        {
            try
            {
                var workflow = HttpContext.Items["Workflow"] as Workflow
                    ?? throw new Exception("Workflow not found in context.");

                if (stream)
                {
                    // Set the response type for SSE
                    Response.ContentType = "text/event-stream";

                    // Get the progress stream from the use case.
                    await foreach (var progress in _processEmbeddingUseCase.ExecuteStreaming(file, workflow))
                    {
                        // Write the current progress as JSON
                        var json = System.Text.Json.JsonSerializer.Serialize(progress);
                        await Response.WriteAsync($"data: {json}\n\n");
                        await Response.Body.FlushAsync();
                    }
                }
                else
                {
                    // Use the traditional synchronous method
                    int processedChunks = await _processEmbeddingUseCase.Execute(file, workflow);
                    await Response.WriteAsJsonAsync(new
                    {
                        Message = "Embedding done successfully.",
                        ProcessedChunks = processedChunks
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.StatusCode = 400;
                await Response.WriteAsync(ex.Message);
            }
        }
    }
}
