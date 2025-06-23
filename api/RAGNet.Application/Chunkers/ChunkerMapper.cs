using RAGNET.Application.Workflows.CreateWorkflow;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Chunkers
{
    public static class ChunkerMapper
    {
        public static Chunker ToChunker(this WorkflowCreationDTO dto, WorkflowId workflowId, string userId)
        {
            return Chunker.Create(
                strategyType: dto.Strategy,
                workflowId: workflowId,
                userId: userId,
                metas:
                [
                    new Meta("threshold", dto.Settings.Threshold.ToString() ),
                    new Meta("evaluationPrompt", dto.Settings.EvaluationPrompt),
                    new Meta("maxChunkSize",  dto.Settings.MaxChunkSize.ToString())
                ]
            );
        }

        public static ChunkerSettingsDTO ToChunkerSettingsDTO(this Dictionary<string, string> meta)
        {
            return new ChunkerSettingsDTO
            {
                Threshold = double.Parse(meta["threshold"]),
                EvaluationPrompt = meta["evaluationPrompt"],
                MaxChunkSize = int.Parse(meta["maxChunkSize"])
            };
        }
    }
}