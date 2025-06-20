using Microsoft.AspNetCore.Mvc;

using RAGNET.Application.Feedbacks;

namespace web.Controllers
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController(ICardCreatorService cardCreatorService) : ControllerBase
    {
        private readonly ICardCreatorService _cardCreatorService = cardCreatorService;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FeedbackRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var description = $"Email: {dto.Email}\n\nMessage:\n{dto.Message}";

                await _cardCreatorService.CreateCardAsync(dto.Name, description);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}