using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.DTOs.Conversation;
using RAGNET.Application.DTOs.Embedder;
using RAGNET.Application.UseCases.UserApiKey;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Enums;
using RAGNET.Infrastructure.Adapters.Chat;
using RAGNET.Infrastructure.Adapters.Embedding;

namespace web.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class AvailableModelsController(
        UserManager<User> userManager,
        IGetUserApiKeysUseCase getUserApiKeysUseCase
    ) : ControllerBase
    {

        private readonly UserManager<User> _userManager = userManager;
        private readonly IGetUserApiKeysUseCase _getUserApiKeysUseCase = getUserApiKeysUseCase;

        [HttpGet("conversation")]
        [Authorize]
        public async Task<IActionResult> GetConversationModels()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var userApiKeys = await _getUserApiKeysUseCase.ExecuteAsync(user.Id);

            if (userApiKeys.Count == 0)
                return Unauthorized("User has no API keys");

            var models = new ConversationModelsDTO();

            foreach (var userApiKey in userApiKeys)
            {
                if (userApiKey.Provider == SupportedProvider.OpenAI)
                    models.OpenAI = OpenAIChatAdapter.GetModels();

                if (userApiKey.Provider == SupportedProvider.Anthropic)
                    models.Anthropic = AnthropicChatAdapter.GetModels();

                if (userApiKey.Provider == SupportedProvider.Gemini)
                {
                    models.Gemini = GeminiChatAdapter.GetModels();
                }
            }
            return Ok(models);
        }

        [HttpGet("embedding")]
        [Authorize]
        public async Task<IActionResult> GetEmbeddingModels()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var userApiKeys = await _getUserApiKeysUseCase.ExecuteAsync(user.Id);

            if (userApiKeys.Count == 0)
                return Unauthorized("User has no API keys");


            var models = new EmbeddingModelsDTO();

            foreach (var userApiKey in userApiKeys)
            {
                if (userApiKey.Provider == SupportedProvider.OpenAI)
                    models.OpenAI = OpenAIEmbeddingAdapter.GetModels();

                if (userApiKey.Provider == SupportedProvider.Voyage)
                    models.Voyage = VoyageEmbeddingAdapter.GetModels();

                if (userApiKey.Provider == SupportedProvider.Gemini)
                {
                    models.Gemini = GeminiEmbeddingAdapter.GetModels();
                }
            }

            return Ok(models);
        }
    }
}
