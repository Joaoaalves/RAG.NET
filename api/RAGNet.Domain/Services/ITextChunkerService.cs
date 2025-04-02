namespace RAGNET.Domain.Services
{
    public interface ITextChunkerService
    {
        /// <summary>
        /// Interface for Chunkers
        /// </summary>
        /// <param name="text">Text to be processed.</param>
        /// <returns>List of chunks.</returns>
        Task<IEnumerable<string>> ChunkText(string text);
    }
}