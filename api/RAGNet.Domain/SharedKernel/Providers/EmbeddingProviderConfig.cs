using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Providers
{

    public class EmbeddingProviderConfig : ValueObject
    {
        public EmbeddingProviderEnum Provider { get; set; }
        public string Model { get; set; } = String.Empty;
        public int VectorSize { get; set; }

        // EF Core ctor
        private EmbeddingProviderConfig() { }

        public EmbeddingProviderConfig(EmbeddingProviderEnum provider, string model, int vectorSize)
        {
            Provider = provider;
            Model = model;
            VectorSize = vectorSize;
        }

        public EmbeddingProviderConfig WithModel(string model) => new(Provider, model, VectorSize);
        public EmbeddingProviderConfig WithProvider(EmbeddingProviderEnum provider) => new(provider, Model, VectorSize);
    }
}