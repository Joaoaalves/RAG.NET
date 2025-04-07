using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.Attributes;
using RAGNET.Application.DTOs.Query;
using RAGNET.Application.UseCases.Query;
using RAGNET.Application.UseCases.QueryEnhancerUseCases;
using RAGNET.Domain.Entities;

namespace web.Controllers
{
    [Route("/api/")]
    [ApiController]
    public class QueryController(
        UserManager<User> userManager,
        IEnhanceQueryUseCase enhanceQueryUseCase,
        IQueryChunksUseCase queryChunksUseCase) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IEnhanceQueryUseCase _enhanceQueryUseCase = enhanceQueryUseCase;
        private readonly IQueryChunksUseCase _queryChunksUseCase = queryChunksUseCase;

        [HttpPost("query")]
        [ApiKeyCheck]
        public async Task<IActionResult> Query([FromBody] QueryDTO queryDTO)
        {
            try
            {
                var apiKey = Request.Headers["x-api-key"].ToString();
                var queries = await _enhanceQueryUseCase.Execute(apiKey, queryDTO);

                var chunks = await _queryChunksUseCase.Execute(apiKey, queries);
                return Ok(new { chunks });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }
}