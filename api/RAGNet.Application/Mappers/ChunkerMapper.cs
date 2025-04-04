using RAGNET.Application.DTOs.Chunker;
using RAGNET.Application.DTOs.Workflow;
using RAGNET.Domain.Entities;

namespace RAGNET.Application.Mappers
{
    public static class ChunkerMapper
    {
        public static Chunker ToChunkerFromWorkflowCreationDTO(this WorkflowCreationDTO dto, Guid workflowId)
        {
            return new Chunker
            {
                WorkflowId = workflowId,
                StrategyType = dto.Strategy,
                Metas =
                [
                    new() { Key = "threshold", Value = dto.Settings.Threshold.ToString() },
                    new() { Key = "evaluationPrompt", Value = dto.Settings.EvaluationPrompt },
                    new() { Key = "maxChunkSize", Value = dto.Settings.MaxChunkSize.ToString() }
                ]
            };
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