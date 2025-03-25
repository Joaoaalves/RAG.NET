using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RAGNET.Application.DTOs.Workflow;
using RAGNET.Domain.Entities;
using RAGNET.Application.UseCases.WorkflowUseCases;
using RAGNET.Application.UseCases.EmbeddingUseCases;
using RAGNET.Application.Attributes;

namespace web.Controllers
{
    [Route("api/workflows")]
    [ApiController]
    public class WorkflowController(
        IGetUserWorkflowsUseCase getUserWorkflowsUseCase,
        ICreateWorkflowUseCase createWorkflowUseCase,
        IGetWorkflowUseCase getWorkflowUseCase,
        IProcessEmbeddingUseCase processEmbeddingUseCase,
        UserManager<User> userManager) : ControllerBase
    {
        private readonly IGetUserWorkflowsUseCase _getUserWorkflowsUseCase = getUserWorkflowsUseCase;
        private readonly ICreateWorkflowUseCase _createWorkflowUseCase = createWorkflowUseCase;
        private readonly IGetWorkflowUseCase _getWorkflowUseCase = getWorkflowUseCase;
        private readonly IProcessEmbeddingUseCase _processEmbeddingUseCase = processEmbeddingUseCase;
        private readonly UserManager<User> _userManager = userManager;

        /// <summary>
        /// Create a Workflow
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowCreationDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var workflowId = await _createWorkflowUseCase.Execute(dto, user);
            return Ok(new { Message = "Workflow criado com sucesso!", WorkflowId = workflowId });
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

        /// <summary>
        /// Returns a workflow with specified id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflow(Guid id)
        {
            try
            {
                var workflowDetails = await _getWorkflowUseCase.Execute(id);
                return Ok(workflowDetails);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Mocks an embedding process
        /// </summary>
        [HttpPost("embedding/{workflow_id}")]
        [Consumes("multipart/form-data")]
        [ApiKeyCheck]
        public async Task<IActionResult> ProcessEmbedding(Guid workflow_id, IFormFile file)
        {
            try
            {
                int processedChunks = await _processEmbeddingUseCase.Execute(workflow_id, file);
                return Ok(new { Message = "Embedding done successfully.", ProcessedChunks = processedChunks });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
