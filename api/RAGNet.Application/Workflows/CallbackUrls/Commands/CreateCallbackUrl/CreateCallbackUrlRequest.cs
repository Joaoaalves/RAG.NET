using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.Workflows.Commands.CreateWorkflow
{
    public class CreateCallbackUrlRequest
    {
        [Required]
        [Url]
        public string Url { get; set; } = String.Empty;
    }
}