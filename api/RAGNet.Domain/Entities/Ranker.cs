using RAGNET.Domain.Enums;

namespace RAGNET.Domain.Entities
{
    public class Ranker
    {
        private string Id { get; set; } = String.Empty;
        private RankerTypeEnum Type { get; set; }
    }
}