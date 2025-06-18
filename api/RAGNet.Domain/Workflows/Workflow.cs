using RAGNET.Domain.Chunkers;
using RAGNET.Domain.Documents;
using RAGNET.Domain.Filters;
using RAGNET.Domain.QueryEnhancers;
using RAGNET.Domain.Rankers;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Providers;
using RAGNET.Domain.SharedKernel.URLs;
using RAGNET.Domain.Workflows.CallbackUrls;

namespace RAGNET.Domain.Workflows
{
    public class Workflow : Entity, IUserOwned, IAggregateRoot
    {
        private readonly List<CallbackUrl> _callbackUrls = [];
        private readonly List<Document> _documents = [];
        private readonly List<QueryEnhancer> _queryEnhancers = [];
        private readonly List<Ranker> _rankers = [];

        public WorkflowId Id { get; private init; } = default!;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        public int DocumentsCount => _documents.Count;

        public string UserId { get; set; } = string.Empty;
        public string ApiKey { get; private set; } = string.Empty;
        public Guid CollectionId { get; private set; }

        public IReadOnlyCollection<CallbackUrl> CallbackUrls => _callbackUrls;
        public IReadOnlyCollection<Document> Documents => _documents;
        public IReadOnlyCollection<QueryEnhancer> QueryEnhancers => _queryEnhancers;
        public IReadOnlyCollection<Ranker> Rankers => _rankers;

        public Chunker? Chunker { get; private set; }
        public ConversationProviderConfig ConversationProviderConfig { get; private set; } = null!;
        public EmbeddingProviderConfig EmbeddingProviderConfig { get; private set; } = null!;
        public Filter? Filter { get; private set; }

        // EF Core
        private Workflow() { }

        private Workflow(
            WorkflowId id,
            string name,
            string description,
            string userId,
            string apiKey,
            Guid collectionId,
            Chunker? chunker,
            ConversationProviderConfig conversationProviderConfig,
            EmbeddingProviderConfig embeddingProviderConfig,
            Filter? filter = null)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = true;
            UserId = userId;
            ApiKey = apiKey;
            CollectionId = collectionId;
            Chunker = chunker;
            ConversationProviderConfig = conversationProviderConfig;
            EmbeddingProviderConfig = embeddingProviderConfig;
            Filter = filter;
        }

        public static Workflow Create(
            string name,
            string description,
            string userId,
            string apiKey,
            Guid collectionId,
            ConversationProviderConfig conversationProviderConfig,
            EmbeddingProviderConfig embeddingProviderConfig,
            Chunker chunker,
            WorkflowId? id = null,
            Filter? filter = null)
        {
            return new Workflow(
                id ?? new WorkflowId(Guid.NewGuid()),
                name,
                description,
                userId,
                apiKey,
                collectionId,
                chunker,
                conversationProviderConfig,
                embeddingProviderConfig,
                filter);
        }

        public void SetActivationState(bool active)
        {
            if (IsActive != active)
                IsActive = active;
        }
        public void Rename(string newName) => Name = newName;
        public void UpdateDescription(string description) => Description = description;
        public void AddCallbackUrl(CallbackUrl callbackUrl)
        {
            CheckRule(new Rules.CallbackUrlsSizeMustBeUnderNRule(_callbackUrls, 5));

            CheckRule(new Rules.CallbackUrlMustBeUniqueForWorkflow(callbackUrl.Url.ToString(), _callbackUrls));

            _callbackUrls.Add(callbackUrl);
        }
        public void UpdateCallbackUrl(Guid callbackId, string newUrl)
        {
            var callback = _callbackUrls.FirstOrDefault(c => c.Id == callbackId) ?? throw new ArgumentException("Callback URL not found.", nameof(callbackId));

            var url = URL.Create(newUrl);

            callback.SetUrl(url);
        }
        public void RemoveCallbackUrl(CallbackUrl callbackUrl)
        {
            _callbackUrls.Remove(callbackUrl);
        }
        public void AddDocument(Document document) => _documents.Add(document);
        public void AddQueryEnhancer(QueryEnhancer enhancer) => _queryEnhancers.Add(enhancer);
        public void AddRanker(Ranker ranker) => _rankers.Add(ranker);
        public void UpdateConversationProviderConfig(ConversationProviderConfig cfg) => ConversationProviderConfig = cfg;
        public void UpdateEmbeddingProviderConfig(EmbeddingProviderConfig cfg) => EmbeddingProviderConfig = cfg;
    }

}
