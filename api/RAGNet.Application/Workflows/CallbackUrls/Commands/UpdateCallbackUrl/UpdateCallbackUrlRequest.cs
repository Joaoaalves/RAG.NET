using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.Workflows.CallbackUrls.Commands.UpdateCallbackUrl
{
    public class UpdateCallbackUrlRequest
    {
        [Required]
        [Url]
        public string Url { get; set; } = string.Empty;
    }
}