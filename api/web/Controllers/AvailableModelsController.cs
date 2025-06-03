using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.Entities;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Application.DTOs.Conversation;
using RAGNET.Application.DTOs.Embedding;
using RAGNET.Application.UseCases.ProviderApiKeyUseCases;

namespace web.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class AvailableModelsController(
        UserManager<User> userManager,
        IGetProviderApiKeysUseCase getProviderApiKeysUseCase,
        IProviderModelCatalogService providerModelCatalogService
    ) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IGetProviderApiKeysUseCase _getProviderApiKeysUseCase = getProviderApiKeysUseCase;
        private readonly IProviderModelCatalogService _providerModelCatalogService = providerModelCatalogService;

        [HttpGet("conversation")]
        [Authorize]
        public async Task<IActionResult> GetConversationModels()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();

            var providerApiKeys = await _getProviderApiKeysUseCase.ExecuteAsync(user.Id);
            if (providerApiKeys.Count == 0)
                return Unauthorized("User has no API keys");

            var availableModels = _providerModelCatalogService.GetConversationModels();
            var response = new ConversationModelsResponseDTO();

            foreach (var apiKey in providerApiKeys)
            {
                if (string.IsNullOrWhiteSpace(apiKey.ApiKey))
                    continue;

                if (availableModels.TryGetValue(apiKey.ProviderId, out var models))
                {
                    response.Providers.Add(new ConversationProviderDTO
                    {
                        ProviderId = apiKey.ProviderId,
                        ProviderName = apiKey.ProviderId,
                        Models = models
                    });
                }
            }

            return Ok(response.Providers);
        }

        [HttpGet("embedding")]
        [Authorize]
        public async Task<IActionResult> GetEmbeddingModels()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();

            var providerApiKeys = await _getProviderApiKeysUseCase.ExecuteAsync(user.Id);
            if (providerApiKeys.Count == 0)
                return Unauthorized("User has no API keys");

            var availableModels = _providerModelCatalogService.GetEmbeddingModels();
            var response = new EmbeddingModelsResponseDTO();

            foreach (var apiKey in providerApiKeys)
            {
                if (string.IsNullOrWhiteSpace(apiKey.ApiKey))
                    continue;

                if (availableModels.TryGetValue(apiKey.ProviderId, out var models))
                {
                    response.Providers.Add(new EmbeddingProviderDTO
                    {
                        ProviderId = apiKey.ProviderId,
                        ProviderName = apiKey.ProviderId,
                        Models = models
                    });
                }
            }

            return Ok(response.Providers);
        }
    }
}
