namespace RAGNET.Application.Configuration
{
    public interface IExecutionContextAcessor
    {
        Guid Correlationid { get; }
        bool IsAvailable { get; }
    }
}