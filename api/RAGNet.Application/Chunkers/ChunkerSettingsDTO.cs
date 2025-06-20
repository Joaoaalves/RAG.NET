namespace RAGNET.Application.Chunkers
{
    public class ChunkerSettingsDTO
    {
        public double Threshold { get; set; }
        public string EvaluationPrompt { get; set; } = String.Empty;
        public int MaxChunkSize { get; set; }
    }
}