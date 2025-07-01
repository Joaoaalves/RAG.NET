using RAGNET.Domain.SharedKernel.Subscriptions;

namespace RAGNET.Domain.SharedKernel.Tokens
{
    public interface ITokenAllowanceStrategy
    {
        TokenAmount GetTokenAmount(PlanType planType);
    }
}