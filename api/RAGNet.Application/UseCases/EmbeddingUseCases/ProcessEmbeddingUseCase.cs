using Microsoft.AspNetCore.Http;
using RAGNET.Domain.Repositories;
using System.Text;

namespace RAGNET.Application.UseCases.EmbeddingUseCases
{
    public interface IProcessEmbeddingUseCase
    {
        Task<int> Execute(Guid workflowId, IFormFile file, string apiKey);
    }

    public class ProcessEmbeddingUseCase(IWorkflowRepository workflowRepository) : IProcessEmbeddingUseCase
    {
        private readonly IWorkflowRepository _workflowRepository = workflowRepository;

        // JUST A MOCK ATM
        public async Task<int> Execute(Guid workflowId, IFormFile file, string apiKey)
        {
            var workflow = await _workflowRepository.GetWithRelationsByApiKey(workflowId, apiKey) ?? throw new Exception("Workflow não encontrado.");
            var chunker = workflow.Chunkers.FirstOrDefault() ?? throw new Exception("Chunker não encontrado.");

            var settings = chunker.Metas.ToDictionary(m => m.Key, m => m.Value);
            var maxChunkSize = int.Parse(settings["maxChunkSize"]);

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var text = Encoding.UTF8.GetString(stream.ToArray());

            var sentences = text.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();
            var chunk = new StringBuilder();

            foreach (var sentence in sentences)
            {
                if (chunk.Length + sentence.Length > maxChunkSize)
                {
                    chunks.Add(chunk.ToString());
                    chunk.Clear();
                }
                chunk.Append(sentence).Append(".");
            }
            if (chunk.Length > 0)
                chunks.Add(chunk.ToString());

            await Task.Delay(1000);

            return chunks.Count;
        }
    }
}
