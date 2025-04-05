using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.QueryEnhancer
{
    public class AutoQueryCreationDTO
    {
        public int MaxQueries { get; set; }
        public string Guidance { get; set; } = String.Empty;
        public bool? IsEnabled { get; set; } = true;
    }

    public class HyDECreationDTO
    {
        public int MaxQueries { get; set; }
        public bool? IsEnabled { get; set; } = true;
    }
}