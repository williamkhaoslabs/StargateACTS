using StargateAPI.Business.Data;

namespace StargateApi.Business.Services;

public class LogService : ILogService
{
    private readonly StargateContext _context;
 
    public LogService(StargateContext context)
    {
        _context = context;
    }
 
    public async Task LogSuccess(string message, string source)
    {
        var log = new ProcessLog
        {
            Message = message,
            LogLevel = "Information",
            Timestamp = DateTime.UtcNow,
            Source = source,
            StackTrace = null
        };
 
        await _context.ProcessLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
 
    public async Task LogException(Exception exception, string source)
    {
        var log = new ProcessLog
        {
            Message = exception.Message,
            LogLevel = "Error",
            Timestamp = DateTime.UtcNow,
            Source = source,
            StackTrace = exception.StackTrace
        };
 
        await _context.ProcessLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}