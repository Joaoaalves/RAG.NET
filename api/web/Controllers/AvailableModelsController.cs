using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.DTOs.Conversation;
using RAGNET.Domain.Services;
using RAGNET.Infrastructure.Adapters.Chat;

namespace web.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class AvailableModelsController() : ControllerBase
    {

        [HttpGet("conversation")]
        [Authorize]
        public IActionResult GetConversationModels()
        {
            var openAIModels = OpenAIChatAdapter.GetModels();
            var anthropicModels = AnthropicChatAdapter.GetModels();

            var models = new ConversationModelsDTO
            {
                OpenAI = openAIModels,
                Anthropic = anthropicModels
            };

            return Ok(models);
        }
    }
}
