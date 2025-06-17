namespace RAGNET.Domain.SharedKernel.Providers
{
    public interface IProviderPolicyFactory
    {
        IProviderPolicy GetPolicy(SupportedProvider type);
    }
}