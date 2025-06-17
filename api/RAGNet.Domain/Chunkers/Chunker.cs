using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.Chunkers
{
    public class Chunker : Entity, IUserOwned
    {
        public readonly List<Meta> _metas = [];
        public Guid Id { get; set; }
        public ChunkerStrategy StrategyType { get; set; }

        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;
        public string UserId { get; set; } = String.Empty;

        public IReadOnlyCollection<Meta> Metas => _metas.AsReadOnly();

        // EF Core ctor
        private Chunker() { }

        private Chunker(Guid id, ChunkerStrategy strategyType, Guid workflowId, string userId, IEnumerable<Meta>? metas = null)
        {
            Id = id;
            StrategyType = strategyType;
            WorkflowId = workflowId;
            UserId = userId;

            if (metas != null)
                _metas.AddRange(metas);
        }

        public static Chunker Create(Guid id, ChunkerStrategy strategyType, Guid workflowId, string userId, IEnumerable<Meta>? metas = null)
        {
            return new Chunker(id, strategyType, workflowId, userId, metas);
        }

        public void UpdateMetas(List<Meta> newMetas)
        {
            _metas.Clear();
            _metas.AddRange(newMetas);
        }
    }
}