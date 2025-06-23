using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.QueryEnhancers
{
    public class QueryEnhancer : Entity, IUserOwned
    {
        private readonly List<Meta> _metas = [];

        public QueryEnhancerId Id { get; private init; } = default!;
        public QueryEnhancerStrategy Type { get; private set; }
        public bool IsEnabled { get; private set; }
        public WorkflowId WorkflowId { get; private set; } = default!;
        public Workflow Workflow { get; private set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public string Prompt { get; private set; } = string.Empty;
        public int MaxQueries { get; private set; }

        public IReadOnlyCollection<Meta> Metas => _metas.AsReadOnly();

        // EF Core ctor
        private QueryEnhancer() { }

        private QueryEnhancer(
            QueryEnhancerId id,
            QueryEnhancerStrategy type,
            WorkflowId workflowId,
            string userId,
            string prompt,
            int maxQueries,
            IEnumerable<Meta>? metas = null,
            bool isEnabled = true)
        {
            Id = id;
            Type = type;
            WorkflowId = workflowId;
            UserId = userId;
            Prompt = prompt;
            MaxQueries = maxQueries;
            IsEnabled = isEnabled;

            if (metas != null)
                _metas.AddRange(metas);
        }

        public static QueryEnhancer Create(
            QueryEnhancerStrategy type,
            WorkflowId workflowId,
            string userId,
            string prompt,
            int maxQueries,
            QueryEnhancerId? id = null,
            IEnumerable<Meta>? metas = null,
            bool isEnabled = true)
        {
            return new QueryEnhancer(id ?? new QueryEnhancerId(), type, workflowId, userId, prompt, maxQueries, metas, isEnabled);
        }

        public void SetEnableState(bool active)
        {
            if (IsEnabled != active)
                IsEnabled = active;
        }

        public void UpdatePrompt(string newPrompt) => Prompt = newPrompt;
        public void UpdateMaxQueries(int maxQueries) => MaxQueries = maxQueries;
        public void UpdateMetas(List<Meta> newMetas)
        {
            _metas.Clear();
            _metas.AddRange(newMetas);
        }
    }
}
