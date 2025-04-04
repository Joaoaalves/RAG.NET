using RAGNET.Domain.Enums;

namespace RAGNET.Application.DTOs.QueryEnhancer
{
    public class AutoQueryCreationDTO
    {
        public int MaxQueries { get; set; }
        public string Guidance { get; set; } = String.Empty;
    }

    public class HyDECreationDTO
    {
        public int MaxQueries { get; set; }
    }

}