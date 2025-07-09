using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Infrastructure.Processing;

using RAGNET.Application.VectorStorages.Queries.GetUserVectorStorages;
using RAGNET.Application.VectorStorages.Queries.GetVectorStoragePolicies;
using RAGNET.Application.VectorStorages.Commands.AddVectorStorage;

namespace web.Controllers.VectorStorages
{
    [Route("api/vector-storages")]
    [ApiController]
    public class VectorStoragesController(
        QueriesExecutor queriesExecutor,
        CommandsExecutor commandsExecutor
    ) : ControllerBase
    {
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        [HttpGet("policies")]
        [Authorize]
        public async Task<IActionResult> GetVectorStoragePolicies()
        {
            try
            {
                var vectorStorages = await _queriesExecutor.Execute(new GetVectorStoragePoliciesQuery());

                return Ok(new { vectorStorages });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddVectorStorage([FromBody] AddVectorStorageRequest request)
        {
            try
            {
                var command = new AddVectorStorageCommand(request);
                var vectorStorage = await _commandsExecutor.Execute(command);

                return Ok(new
                {
                    vectorStorage
                });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserVectorStorages()
        {
            try
            {
                var vectorStorages = await _queriesExecutor.Execute(new GetUserVectorStoragesQuery());

                return Ok(new { vectorStorages });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }

}