using Microsoft.AspNetCore.Mvc;
using RAGNET.Application.DTOs.Feedback;
using RAGNET.Domain.Entities;
using RAGNET.Domain.Services;

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

                var card = new Card
                {
                    Name = dto.Name,
                    Description = description
                };

                await _cardCreatorService.CreateCardAsync(card);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}