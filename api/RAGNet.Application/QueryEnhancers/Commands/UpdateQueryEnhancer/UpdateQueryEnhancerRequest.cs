using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.QueryEnhancers.Commands.UpdateQueryEnhancer
{
    public class UpdateQueryEnhancerRequest
    {
        [Required]
        public int MaxQueries { get; set; }

        public bool? IsEnabled { get; set; } = true;
    }
    public class UpdateAutoQueryRequest : UpdateQueryEnhancerRequest
    {
        [Required]
        public string Guidance { get; set; } = String.Empty;
    }

    public class UpdateHyDERequest : UpdateQueryEnhancerRequest
    {
    }
}