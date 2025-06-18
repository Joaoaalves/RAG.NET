using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Filters;
using RAGNET.Domain.SharedKernel.Providers;

namespace RAGNET.Domain.Workflows
{
    public class WorkflowBuilder
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _userId = string.Empty;
        private string _apiKey = string.Empty;
        private Guid _collectionId = Guid.NewGuid();
        private Chunker _chunker = null!;
        private ConversationProviderConfig _conversationProvider = null!;
        private EmbeddingProviderConfig _embeddingProvider = null!;
        private Filter? _filter;

        public WorkflowBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public WorkflowBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public WorkflowBuilder ForUser(string userId)
        {
            _userId = userId;
            return this;
        }

        public WorkflowBuilder WithApiKey(string apiKey)
        {
            _apiKey = apiKey;
            return this;
        }

        public WorkflowBuilder WithCollectionId(Guid collectionId)
        {
            _collectionId = collectionId;
            return this;
        }

        public WorkflowBuilder WithChunker(Chunker chunker)
        {
            _chunker = chunker;
            return this;
        }

        public WorkflowBuilder WithConversationProvider(ConversationProviderConfig config)
        {
            _conversationProvider = config;
            return this;
        }

        public WorkflowBuilder WithEmbeddingProvider(EmbeddingProviderConfig config)
        {
            _embeddingProvider = config;
            return this;
        }

        public WorkflowBuilder WithFilter(Filter? filter)
        {
            _filter = filter;
            return this;
        }

        public Workflow Build(WorkflowId? id)
        {
            return Workflow.Create(
                _name,
                _description,
                _userId,
                _apiKey,
                _collectionId,
                _conversationProvider,
                _embeddingProvider,
                _chunker,
                id ?? new WorkflowId(Guid.NewGuid()),
                _filter
            );
        }
    }

}