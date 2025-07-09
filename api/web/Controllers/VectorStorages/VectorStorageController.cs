using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Infrastructure.Processing;

using RAGNET.Application.VectorStorages.Queries.GetUserVectorStorages;
using RAGNET.Application.VectorStorages.Queries.GetVectorStoragePolicies;
using RAGNET.Application.VectorStorages.Commands.AddVectorStorage;
using RAGNET.Application.VectorStorages.Commands.DeleteVectorStorage;
using RAGNET.Domain.VectorStorages;
using RAGNET.Application.VectorStorages.Commands.UpdateVectorStorage;

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

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVectorStorage([FromBody] UpdateVectorStorageRequest request, [FromRoute] Guid id)
        {
            try
            {
                var command = new UpdateVectorStorageCommand(request, new VectorStorageId(id));
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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> AddVectorStorage([FromRoute] Guid id)
        {
            try
            {
                var command = new DeleteVectorStorageCommand(
                    new VectorStorageId(id)
                );

                var success = await _commandsExecutor.Execute(command);

                return Ok(new
                {
                    Message = "Vector Storage was removed!"
                });
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }
        }
    }

}