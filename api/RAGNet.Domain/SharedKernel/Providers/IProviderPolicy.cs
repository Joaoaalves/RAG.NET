namespace RAGNET.Domain.SharedKernel.Providers
{
    public interface IProviderPolicy
    {
        SupportedProvider ProviderType { get; }
        string Name { get; }
        string Prefix { get; }
        string Pattern { get; }
        string Url { get; }
        void Validate(string apiKey);
    }
}