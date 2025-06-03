namespace RAGNET.Domain.SharedKernel.Providers
{
    public interface IProviderPolicy
    {
        SupportedProvider Type { get; }
        void Validate(string apiKey);
    }
}