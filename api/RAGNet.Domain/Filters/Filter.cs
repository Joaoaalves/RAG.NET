using RAGNET.Domain.Workflows;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;

namespace RAGNET.Domain.Filters
{
    public class Filter : Entity, IUserOwned
    {
        private readonly List<Meta> _metas = [];

        public Guid Id { get; set; }
        public FilterStrategyEnum Strategy { get; private set; }
        public Workflow Workflow { get; private set; } = null!;
        public WorkflowId WorkflowId { get; set; } = null!;
        public string UserId { get; set; } = String.Empty;
        public int MaxItems { get; private set; } = 5;
        public bool IsEnabled { get; private set; } = false;
        public IReadOnlyCollection<Meta> Metas { get; set; } = [];

        // EF Core Ctor
        private Filter() { }

        private Filter(
            Guid id,
            FilterStrategyEnum strategy,
            WorkflowId workflowId,
            string userId,
            int maxItems = 5,
            IEnumerable<Meta>? metas = null,
            bool isEnabled = false)
        {
            Id = id;
            Strategy = strategy;
            WorkflowId = workflowId;
            UserId = userId;
            MaxItems = maxItems;
            IsEnabled = isEnabled;

            if (metas != null)
                _metas.AddRange(metas);
        }

        public static Filter Create(
            Guid id,
            FilterStrategyEnum strategy,
            WorkflowId workflowId,
            string userId,
            int maxItems = 5,
            IEnumerable<Meta>? metas = null,
            bool isEnabled = false)
        {
            return new Filter(id, strategy, workflowId, userId, maxItems, metas, isEnabled);
        }

        public void UpdateMetas(List<Meta> metas)
        {
            _metas.Clear();
            _metas.AddRange(metas);
            Metas = _metas.AsReadOnly();
        }

        public void SetEnableState(bool active)
        {
            if (IsEnabled != active)
            {
                IsEnabled = active;
            }
        }

        public void UpdateMaxItems(int maxItems)
        {
            if (maxItems <= 0)
            {
                throw new ArgumentException("Max items must be greater than zero.", nameof(maxItems));
            }

            MaxItems = maxItems;
        }

        public void UpdateStrategy(FilterStrategyEnum strategy)
        {
            if (!Enum.IsDefined(strategy))
            {
                throw new ArgumentOutOfRangeException(nameof(strategy), "Invalid filter strategy.");
            }

            Strategy = strategy;
        }
    }
}