using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Providers
{

    public class ConversationProviderConfig : ValueObject
    {
        public ConversationProviderEnum Provider { get; }
        public string Model { get; } = String.Empty;

        // EF Core ctor
        private ConversationProviderConfig() { }
        public ConversationProviderConfig(ConversationProviderEnum provider, string model)
        {
            Provider = provider;
            Model = model;
        }

        public ConversationProviderConfig WithModel(string model) => new(Provider, model);
        public ConversationProviderConfig WithProvider(ConversationProviderEnum provider) => new(provider, Model);
    }
}