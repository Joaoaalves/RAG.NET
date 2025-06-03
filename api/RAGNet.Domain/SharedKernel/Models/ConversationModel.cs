using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Models
{
    public class ConversationModel : ValueObject
    {
        public string Label { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
        public int Speed { get; init; }
        public float InputPrice { get; init; }
        public float OutputPrice { get; init; }
        public int MaxOutput { get; init; }
        public int ContextWindow { get; init; }
    }
}
