using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.Workflows.CallbackUrls.Commands.CreateCallbackUrl
{
    public class CreateCallbackUrlRequest
    {
        [Required]
        [Url]
        public string Url { get; set; } = String.Empty;
    }
}