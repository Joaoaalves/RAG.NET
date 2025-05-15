namespace RAGNET.Application.DTOs.Feedback
{
    public class FeedbackRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
    }
}