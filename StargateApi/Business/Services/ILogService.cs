namespace StargateApi.Business.Services;

public interface ILogService
{
    Task LogSuccess(string message, string source);
    Task LogException(Exception exception, string source);
}