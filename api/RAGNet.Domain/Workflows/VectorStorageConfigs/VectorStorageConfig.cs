using RAGNET.Domain.SeedWork;
using RAGNET.Domain.VectorStorages;

namespace RAGNET.Domain.Workflows.VectorStorageConfigs
{
    public class VectorStorageConfig : Entity
    {
        public VectorStorageConfigId Id { get; private init; } = null!;
        public Guid CollectionId { get; private set; }
        public uint VectorDimension { get; private set; }

        public Workflow Workflow { get; private set; } = null!;
        public WorkflowId WorkflowId { get; private set; } = null!;
        public VectorStorageId VectorStorageId { get; private set; } = null!;
        public VectorStorage VectorStorage { get; private set; } = null!;

        private VectorStorageConfig() { }

        private VectorStorageConfig(
            VectorStorageConfigId id,
            VectorStorageId vectorStorageId,
            Guid collectionId,
            uint vectorDimension,
            WorkflowId workflowId)
        {
            Id = id;
            VectorStorageId = vectorStorageId;
            CollectionId = collectionId;
            VectorDimension = vectorDimension;
            WorkflowId = workflowId;
        }

        public static VectorStorageConfig Create(
            VectorStorageId vectorStorageId,
            Guid collectionId,
            uint vectorDimension,
            WorkflowId workflowId,
            VectorStorageConfigId? id = null)
        {
            VectorStorageConfigId validId = id ?? new();

            return new VectorStorageConfig(
                validId,
                vectorStorageId,
                collectionId,
                vectorDimension,
                workflowId
            );
        }
    }

}