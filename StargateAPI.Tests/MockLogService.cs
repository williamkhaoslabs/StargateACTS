using StargateApi.Business.Services;

namespace StargateAPI.Tests
{
    public class MockLogService : ILogService
    {
        public List<(string Message, string Source)> SuccessLogs { get; } = new();
        public List<(Exception Exception, string Source)> ExceptionLogs { get; } = new();

        public Task LogSuccess(string message, string source)
        {
            SuccessLogs.Add((message, source));
            return Task.CompletedTask;
        }

        public Task LogException(Exception exception, string source)
        {
            ExceptionLogs.Add((exception, source));
            return Task.CompletedTask;
        }
    }
}