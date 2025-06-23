using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.ProvidersApiKeys;

using RAGNET.Application.ProviderApiKeys.Commands.CreateProviderApiKey;
using RAGNET.Application.ProviderApiKeys.Queries.GetUserProviderApiKeys;
using RAGNET.Application.ProviderApiKeys.Commands.UpdateProviderApiKey;
using RAGNET.Application.ProviderApiKeys.Commands.DeleteProviderApiKey;

using RAGNET.Infrastructure.Processing;

namespace web.Controllers.Providers
{
    public class ProviderController(
        CommandsExecutor commandsExecutor,
        QueriesExecutor queriesExecutor
    ) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;

        [HttpPost("/api/provider")]
        [Authorize]
        public async Task<IActionResult> CreateProviderApiKey([FromBody] CreateProviderApiKeyRequest request)
        {
            try
            {
                var command = new CreateProviderApiKeyCommand(
                    request.Provider,
                    request.ApiKey
                );

                var result = await _commandsExecutor.Execute(command);

                if (result == null)
                    return BadRequest("Error creating user API key");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/provider")]
        [Authorize]
        public async Task<IActionResult> GetProviderApiKeys()
        {
            try
            {
                var query = new GetUserProviderApiKeysQuery();

                var result = await _queriesExecutor.Execute(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/provider/{providerId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProviderApiKey([FromBody] UpdateProviderApiKeyRequest request, [FromRoute] Guid providerId)
        {
            try
            {
                var command = new UpdateProviderApiKeyCommand(
                    new ProviderApiKeyId(providerId),
                    request.ApiKey
                );

                var result = await _commandsExecutor.Execute(command);

                return Ok(result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpDelete("/api/provider/{providerId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProviderApiKey([FromRoute] Guid providerId)
        {
            try
            {
                var command = new DeleteProviderApiKeyCommand(
                    new ProviderApiKeyId(providerId)
                );

                var result = await _commandsExecutor.Execute(command);

                return Ok(result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}