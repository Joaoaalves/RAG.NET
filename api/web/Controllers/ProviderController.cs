using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.Entities;

using RAGNET.Application.DTOs.ProviderApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.ProviderApiKeyUseCases;

namespace web.Controllers
{
    public class ProviderController(
        ICreateProviderApiKeyUseCase createProviderApiKeyUseCase,
        IGetProviderApiKeysUseCase getProviderApiKeyUseCase,
        IUpdateProviderApiKeyUseCase updateProviderApiKeyUseCase,
        IDeleteProviderApiKeyUseCase deleteProviderApiKeyUseCase,
        UserManager<User> userManager
    ) : ControllerBase
    {
        private readonly ICreateProviderApiKeyUseCase _createProviderApiKeyUseCase = createProviderApiKeyUseCase;
        private readonly IGetProviderApiKeysUseCase _getProviderApiKeyUseCase = getProviderApiKeyUseCase;
        private readonly IUpdateProviderApiKeyUseCase _updateProviderApiKeyUseCase = updateProviderApiKeyUseCase;
        private readonly IDeleteProviderApiKeyUseCase _deleteProviderApiKeyUseCase = deleteProviderApiKeyUseCase;
        readonly UserManager<User> _userManager = userManager;

        [HttpPost("/api/provider")]
        [Authorize]
        public async Task<IActionResult> CreateProviderApiKey([FromBody] CreateProviderApiKeyDTO dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var result = await _createProviderApiKeyUseCase.ExecuteAsync(dto, user.Id);

                if (result == null)
                    return BadRequest("Error creating user API key");

                return Ok(result.ToDTO());
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

                var result = await _getProviderApiKeyUseCase.ExecuteAsync(user.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/provider/{providerId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProviderApiKey([FromBody] UpdateProviderApiKeyDTO dto, [FromRoute] Guid providerId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var result = await _updateProviderApiKeyUseCase.ExecuteAsync(dto, providerId, user.Id);
                return Ok(result.ToDTO());
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


                var result = await _deleteProviderApiKeyUseCase.ExecuteAsync(providerId, user.Id);
                return Ok(result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}