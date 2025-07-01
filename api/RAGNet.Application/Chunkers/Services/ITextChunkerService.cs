namespace RAGNET.Application.Chunkers.Services
{
    public interface ITextChunkerService
    {
        /// <summary>
        /// Interface for Chunkers
        /// </summary>
        /// <param name="text">Text to be processed.</param>
        /// <returns>List of chunks.</returns>
        Task<List<string>> ChunkText(string text);
        decimal GetCostMultiplier();
    }
}