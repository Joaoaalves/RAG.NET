using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.Users;

using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Infrastructure.Processing;
using RAGNET.Application.ProviderApiKeys.CreateProviderApiKey;
using RAGNET.Application.ProviderApiKeys.GetUserProviderApiKeys;
using RAGNET.Application.ProviderApiKeys.UpdateProviderApiKey;
using RAGNET.Application.ProviderApiKeys.DeleteProviderApiKey;

namespace web.Controllers.Providers
{
    public class ProviderController(
        CommandsExecutor commandsExecutor,
        QueriesExecutor queriesExecutor,
        UserManager<User> userManager
    ) : ControllerBase
    {
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;
        readonly UserManager<User> _userManager = userManager;

        [HttpPost("/api/provider")]
        [Authorize]
        public async Task<IActionResult> CreateProviderApiKey([FromBody] CreateProviderApiKeyRequest request)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var command = new CreateProviderApiKeyCommand(
                    user.Id,
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
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var query = new GetUserProviderApiKeysQuery(
                    user.Id
                );

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
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var command = new UpdateProviderApiKeyCommand(
                    user.Id,
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
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var command = new DeleteProviderApiKeyCommand(
                    user.Id,
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