using RAGNET.Domain.Enums;
using RAGNET.Domain.SharedKernel.Metas;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.QueryEnhancers
{
    public class QueryEnhancerBuilder
    {
        private Guid _id = Guid.NewGuid();
        private QueryEnhancerStrategy _type = QueryEnhancerStrategy.AUTO_QUERY;
        private Guid _workflowId = default!;
        private string _userId = string.Empty;
        private string _prompt = string.Empty;
        private int _maxQueries = 10;
        private bool _enabled = true;
        private List<Meta> _metas = [];

        public QueryEnhancerBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public QueryEnhancerBuilder WithType(QueryEnhancerStrategy type)
        {
            _type = type;
            return this;
        }

        public QueryEnhancerBuilder WithWorkflowId(Guid workflowId)
        {
            _workflowId = workflowId;
            return this;
        }

        public QueryEnhancerBuilder ForUser(string userId)
        {
            _userId = userId;
            return this;
        }

        public QueryEnhancerBuilder WithPrompt(string prompt)
        {
            _prompt = prompt;
            return this;
        }

        public QueryEnhancerBuilder WithMaxQueries(int maxQueries)
        {
            _maxQueries = maxQueries;
            return this;
        }

        public QueryEnhancerBuilder WithMetas(IEnumerable<Meta> metas)
        {
            _metas = [.. metas];
            return this;
        }

        public QueryEnhancerBuilder Enabled(bool enabled)
        {
            _enabled = enabled;
            return this;
        }

        public QueryEnhancer Build()
        {
            return QueryEnhancer.Create(
                _id,
                _type,
                _workflowId,
                _userId,
                _prompt,
                _maxQueries,
                _metas,
                _enabled
            );
        }
    }
}
