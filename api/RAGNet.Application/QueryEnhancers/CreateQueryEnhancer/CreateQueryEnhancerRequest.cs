using System.ComponentModel.DataAnnotations;

namespace RAGNET.Application.QueryEnhancers.CreateQueryEnhancer
{
    public class CreateAutoQueryRequest
    {
        [Required]
        public int MaxQueries { get; set; }
        [Required]
        public string Guidance { get; set; } = String.Empty;
        public bool? IsEnabled { get; set; } = true;
    }

    public class CreateHyDERequest
    {
        [Required]
        public int MaxQueries { get; set; }
        public bool? IsEnabled { get; set; } = true;
    }
}