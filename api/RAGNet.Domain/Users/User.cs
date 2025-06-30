using Microsoft.AspNetCore.Identity;
using RAGNET.Domain.ProvidersApiKeys;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.SharedKernel.Subscriptions;
using RAGNET.Domain.SharedKernel.Users;
using RAGNET.Domain.TokenWallets;
using RAGNET.Domain.Users.Subscriptions;
using RAGNET.Domain.Workflows;

namespace RAGNET.Domain.Users
{
    public class User : IdentityUser, IAggregateRoot
    {
        private readonly List<Workflow> _workflows = [];
        private readonly List<ProviderApiKey> _apiKeys = [];

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public TokenWallet TokenWallet { get; private set; } = null!;
        public Subscription Subscription { get; private set; } = null!;

        // Payment Gateway
        public string CustomerId { get; private set; } = null!;

        public IReadOnlyCollection<Workflow> Workflows => _workflows.AsReadOnly();
        public IReadOnlyCollection<ProviderApiKey> ApiKeys => _apiKeys.AsReadOnly();

        // Required by EF Core
        [Obsolete("Only for EF/Identity serialization", true)]
        public User() { }

        private User(Name firstName, Name lastName, UserName userName, Email email)
        {
            // Identity Conflicts with our properties, so we use base properties
            FirstName = firstName.Value;
            LastName = lastName.Value;

            base.Email = email.Value;
            base.UserName = userName.Value;
        }

        public static User Create(Name firstName, Name lastName, UserName userName, Email email)
        {
            return new User(firstName, lastName, userName, email);
        }

        public void AddWorkflow(Workflow workflow)
        {
            ArgumentNullException.ThrowIfNull(workflow);
            _workflows.Add(workflow);
            workflow.UserId = Id;
        }

        public void AddApiKey(ProviderApiKey apiKey)
        {
            ArgumentNullException.ThrowIfNull(apiKey);
            _apiKeys.Add(apiKey);
            apiKey.UserId = Id;
        }

        public void AddWallet(TokenWallet wallet)
        {
            ArgumentNullException.ThrowIfNull(wallet);
            TokenWallet = wallet;
        }

        public void AddSubscription(Subscription subscription)
        {
            ArgumentNullException.ThrowIfNull(subscription);
            Subscription = subscription;
        }

        public void AddCustomerId(string customerId)
        {
            CustomerId = customerId;
        }

        public void RemoveWorkflow(Workflow workflow)
        {
            ArgumentNullException.ThrowIfNull(workflow);
            if (!_workflows.Remove(workflow))
                throw new InvalidOperationException("Workflow not found in user's workflows.");
        }

        public void RemoveApiKey(ProviderApiKey apiKey)
        {
            ArgumentNullException.ThrowIfNull(apiKey);
            if (!_apiKeys.Remove(apiKey))
                throw new InvalidOperationException("API key not found in user's API keys.");
        }
    }

}

