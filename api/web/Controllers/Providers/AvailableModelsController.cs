using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Domain.SharedKernel.Providers;

using RAGNET.Infrastructure.Processing;
using RAGNET.Application.ProviderApiKeys.Queries.GetUserProviderApiKeys;
using RAGNET.Application.Infrastructure.Providers.Embedding.DTOs;
using RAGNET.Application.Infrastructure.Providers.Conversation.DTOs;

namespace web.Controllers.Providers
{
    [Route("api/models")]
    [ApiController]
    public class AvailableModelsController(
        QueriesExecutor queriesExecutor,
        IProviderModelCatalogService providerModelCatalogService
    ) : ControllerBase
    {
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;
        private readonly IProviderModelCatalogService _providerModelCatalogService = providerModelCatalogService;

        [HttpGet("conversation")]
        [Authorize]
        public async Task<IActionResult> GetConversationModels()
        {
            var userProviderApiQuery = new GetUserProviderApiKeysQuery();
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
            var userProviderApiQuery = new GetUserProviderApiKeysQuery();
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
