using RAGNET.Domain.SharedKernel.Tokens;

namespace RAGNET.Application.TokenWallets.Services.Consumption
{
    public interface ITokenConsumptionStrategy
    {
        string Operation { get; }
        string GetContextInfo();
        string GetUserId();
        TokenAmount CalculateCost();
    }
}