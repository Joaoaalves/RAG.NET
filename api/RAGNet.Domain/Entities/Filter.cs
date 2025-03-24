using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{
    public class Filter
    {
        private string Id { get; set; } = String.Empty;
        private FilterTypeEnum Type { get; set; }
        private float Threshold { get; set; } = 0.9F;
        private int MaxChunks { get; set; } = 6;
    }
}