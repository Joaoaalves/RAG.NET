using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.UserApiKey;
using RAGNET.Application.Mappers;
using RAGNET.Application.UseCases.UserApiKey;
using RAGNET.Domain.Entities;

namespace web.Controllers
{
    public class ProviderController(
        ICreateUserApiKeyUseCase createUserApiKeyUseCase,
        IGetUserApiKeysUseCase getUserApiKeyUseCase,
        IUpdateUserApiKeyUseCase updateUserApiKeyUseCase,
        IDeleteUserApiKeyUseCase deleteUserApiKeyUseCase,
        UserManager<User> userManager
    ) : ControllerBase
    {
        private readonly ICreateUserApiKeyUseCase _createUserApiKeyUseCase = createUserApiKeyUseCase;
        private readonly IGetUserApiKeysUseCase _getUserApiKeyUseCase = getUserApiKeyUseCase;
        private readonly IUpdateUserApiKeyUseCase _updateUserApiKeyUseCase = updateUserApiKeyUseCase;
        private readonly IDeleteUserApiKeyUseCase _deleteUserApiKeyUseCase = deleteUserApiKeyUseCase;
        readonly UserManager<User> _userManager = userManager;

        [HttpPost("/api/provider")]
        [Authorize]
        public async Task<IActionResult> CreateUserApiKey([FromBody] CreateUserApiKeyDTO dto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var result = await _createUserApiKeyUseCase.ExecuteAsync(dto, user.Id);

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
        public async Task<IActionResult> GetUserApiKeys()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var result = await _getUserApiKeyUseCase.ExecuteAsync(user.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/provider/{providerId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserApiKey([FromBody] UpdateUserApiKeyDTO dto, [FromRoute] Guid providerId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    return Unauthorized();

                var result = await _updateUserApiKeyUseCase.ExecuteAsync(dto, providerId, user.Id);
                return Ok(result.ToDTO());
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpDelete("/api/provider/{providerId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserApiKey([FromRoute] Guid providerId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();


                var result = await _deleteUserApiKeyUseCase.ExecuteAsync(providerId, user.Id);
                return Ok(result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}