using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.Users;
using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Infrastructure.Processing;
using RAGNET.Application.ProviderApiKeys.GetUserProviderApiKeys;
using RAGNET.Application.Providers.Conversation;
using RAGNET.Application.Providers.Embedding;

namespace web.Controllers.Providers
{
    [Route("api/models")]
    [ApiController]
    public class AvailableModelsController(
        UserManager<User> userManager,
        QueriesExecutor queriesExecutor,
        IProviderModelCatalogService providerModelCatalogService
    ) : ControllerBase
    {
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IProviderModelCatalogService _providerModelCatalogService = providerModelCatalogService;

        [HttpGet("conversation")]
        [Authorize]
        public async Task<IActionResult> GetConversationModels()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();

            var userProviderApiQuery = new GetUserProviderApiKeysQuery(user.Id);
            var providerApiKeys = await _queriesExecutor.Execute(userProviderApiQuery);

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

            var userProviderApiQuery = new GetUserProviderApiKeysQuery(user.Id);
            var providerApiKeys = await _queriesExecutor.Execute(userProviderApiQuery);

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
