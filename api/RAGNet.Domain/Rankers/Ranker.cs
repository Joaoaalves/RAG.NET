using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.Rankers
{
    public class Ranker : Entity, IUserOwned
    {

        private readonly List<Meta> _metas = [];

        public RankerId Id { get; private init; } = default!;
        public string UserId { get; set; } = string.Empty;
        public WorkflowId WorkflowId { get; set; } = null!;
        public Workflow Workflow { get; set; } = null!;
        public bool IsEnabled { get; set; } = true;
        public IReadOnlyCollection<Meta> Metas => _metas.AsReadOnly();

        // EF Core ctor
        private Ranker() { }

        private Ranker(
            RankerId id,
            string userId,
            bool isEnabled,
            IEnumerable<Meta>? metas = null
        )
        {
            Id = id;
            UserId = userId;
            IsEnabled = isEnabled;

            if (metas != null)
                _metas.AddRange(metas);
        }

        public static Ranker Create(
            string userId,
            bool isEnabled,
            RankerId? id = null,
            IEnumerable<Meta>? metas = null
        )
        {
            return new Ranker(id ?? new RankerId(), userId, isEnabled, metas);
        }

        public void SetEnableState(bool active)
        {
            if (IsEnabled != active)
                IsEnabled = active;
        }
        public void UpdateMetas(List<Meta> newMetas)
        {
            _metas.Clear();
            _metas.AddRange(newMetas);
        }
    }
}