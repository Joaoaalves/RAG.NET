namespace RAGNET.Domain.Entities
{
    public class ConversationModel
    {
        public string Label { get; set; } = String.Empty;
        public string Value { get; set; } = String.Empty;
        public int Speed { get; set; } = 0;
        public float InputPrice { get; set; } = 0;
        public float OutputPrice { get; set; } = 0;
        public int MaxOutput { get; set; } = 0;
        public int ContextWindow { get; set; } = 0;
    }
}