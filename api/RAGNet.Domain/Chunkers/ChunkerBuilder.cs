using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.Chunkers
{
    public class ChunkerBuilder
    {
        private ChunkerStrategy _strategy = ChunkerStrategy.PARAGRAPH;
        private WorkflowId _workflowId = null!;
        private string _userId = null!;
        private List<Meta> _metas = [];

        public ChunkerBuilder WithStrategy(ChunkerStrategy strategy)
        {
            _strategy = strategy;
            return this;
        }

        public ChunkerBuilder ForWorkflow(WorkflowId workflowId)
        {
            _workflowId = workflowId;
            return this;
        }

        public ChunkerBuilder ForUser(string userId)
        {
            _userId = userId;
            return this;
        }

        public ChunkerBuilder AddMeta(Meta meta)
        {
            _metas.Add(meta);
            return this;
        }

        public Chunker Build()
        {
            return Chunker.Create(
                _strategy,
                _workflowId,
                _userId,
                _metas
            );
        }
    }
}