using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.Workflow;
using RAGNET.Domain.Chunkers;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Application.Mappers
{
    public static class ChunkerMapper
    {
        public static Chunker ToChunkerFromWorkflowCreationDTO(this WorkflowCreationDTO dto, Guid workflowId, string userId)
        {
            return Chunker.Create(
                id: Guid.NewGuid(),
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

        public static ChunkerSettingsDTO ToChunkerSettingsDTOfromDictionary(this Dictionary<string, string> meta)
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