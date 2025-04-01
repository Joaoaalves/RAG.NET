using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.DTOs.Conversation;
using RAGNET.Application.DTOs.Embedder;

using RAGNET.Infrastructure.Adapters.Chat;
using RAGNET.Infrastructure.Adapters.Embedding;

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

        [HttpGet("embedding")]
        [Authorize]
        public IActionResult GetEmbeddingModels()
        {
            var openAIModels = OpenAIEmbeddingAdapter.GetModels();
            var anthropicModels = VoyageEmbeddingAdapter.GetModels();

            var models = new EmbeddingModelsDTO
            {
                OpenAI = openAIModels,
                Anthropic = anthropicModels
            };

            return Ok(models);
        }
    }
}
