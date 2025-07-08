namespace RAGNET.Domain.SharedKernel.Providers
{
    public interface IProviderPolicyFactory
    {
        IProviderPolicy CreatePolicy(SupportedProvider type);
    }
}