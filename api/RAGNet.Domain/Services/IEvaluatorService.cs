namespace RAGNET.Domain.Services
{
    public interface IEvaluator
    {
        Task<bool> EvaluateAsync(string proposition, int Threshold);
    }
}