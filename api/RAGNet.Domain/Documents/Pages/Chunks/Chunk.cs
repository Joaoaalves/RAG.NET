namespace RAGNET.Domain.Documents.Pages.Chunks
{
    public class Chunk
    {
        public ChunkId Id { get; private init; } = default!;
        public Text Text { get; set; } = default!;
        public string VectorId { get; set; } = String.Empty;
        public PageId PageId { get; set; } = default!;
        public Page Page { get; set; } = null!;

        // Not mapped to the database
        private SemanticVector? Vector;
        private Score? Score;

        // EF Core ctor
        public Chunk() { }

        public Chunk(ChunkId id, Text text, string vectorId, PageId pageId, SemanticVector vector, Score score)
        {
            Id = id;
            Text = text;
            VectorId = vectorId;
            PageId = pageId;
            Vector = vector;
            Score = score;
        }

        public static Chunk Create(Text text, string vectorId, PageId pageId, ChunkId? id = null, SemanticVector? vector = null, Score? score = null)
        {
            return new Chunk(
                id ?? new ChunkId(),
                text,
                vectorId,
                pageId,
                vector ?? new SemanticVector([]),
                score: score ?? new Score(0.0));
        }

        public void SetEmbedding(SemanticVector embedding)
        {
            Vector = embedding;
        }

        public void SetScore(Score score)
        {
            Score = score;
        }

        public string? GetVector()
        {
            return Vector?.ToString();
        }

        public double GetScore()
        {
            return Score?.Value ?? 0.0;
        }
    }
}