using Microsoft.EntityFrameworkCore;
using StargateApi.Business.Services;

namespace StargateAPI.Tests;

public class LogServiceTests
{
    [Fact]
    public async Task LogSuccess_CreatesInformationLogRow()
    {
        using var context = TestDbContextFactory.Create();
        var logService = new LogService(context);

        await logService.LogSuccess("Person created: John Doe", "CreatePersonHandler");

        var log = await context.ProcessLogs.FirstOrDefaultAsync();
        Assert.NotNull(log);
        Assert.Equal("Information", log.LogLevel);
        Assert.Equal("Person created: John Doe", log.Message);
        Assert.Equal("CreatePersonHandler", log.Source);
        Assert.Null(log.StackTrace);
        Assert.True(log.Timestamp > DateTime.MinValue);
    }

    [Fact]
    public async Task LogException_CreatesErrorLogRowWithStackTrace()
    {
        using var context = TestDbContextFactory.Create();
        var logService = new LogService(context);

        Exception capturedException;
        try
        {
            throw new InvalidOperationException("Something went wrong");
        }
        catch (Exception ex)
        {
            capturedException = ex;
        }

        await logService.LogException(capturedException, "ExceptionHandlingMiddleware");

        var log = await context.ProcessLogs.FirstOrDefaultAsync();
        Assert.NotNull(log);
        Assert.Equal("Error", log.LogLevel);
        Assert.Equal("Something went wrong", log.Message);
        Assert.Equal("ExceptionHandlingMiddleware", log.Source);
        Assert.NotNull(log.StackTrace);
        Assert.Contains("at StargateAPI.Tests", log.StackTrace);
    }

    [Fact]
    public async Task MultipleLogCalls_AllPersisted()
    {
        using var context = TestDbContextFactory.Create();
        var logService = new LogService(context);

        await logService.LogSuccess("First", "HandlerA");
        await logService.LogSuccess("Second", "HandlerB");
        await logService.LogSuccess("Third", "HandlerC");

        var count = await context.ProcessLogs.CountAsync();
        Assert.Equal(3, count);
    }
}