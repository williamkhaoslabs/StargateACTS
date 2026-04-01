using System.Net;
using System.Text.Json;
using API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StargateApi.Business.Services;
using StargateAPI.Controllers;

namespace StargateAPI.Tests;

public class ExceptionHandlingMiddlewareTests
{
    private static ExceptionHandlingMiddleware CreateMiddleware(RequestDelegate next)
    {
        var logger = LoggerFactory
            .Create(b => b.AddDebug())
            .CreateLogger<ExceptionHandlingMiddleware>();

        return new ExceptionHandlingMiddleware(next, logger);
    }

    private static HttpContext CreateHttpContext(ILogService? logService = null)
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        if (logService != null)
        {
            var services = new ServiceCollection();
            services.AddSingleton(logService);
            context.RequestServices = services.BuildServiceProvider();
        }
        else
        {
            context.RequestServices = new ServiceCollection().BuildServiceProvider();
        }

        return context;
    }

    private static async Task<BaseResponse?> ReadResponseBody(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return await JsonSerializer.DeserializeAsync<BaseResponse>(context.Response.Body, options);
    }

    [Fact]
    public async Task InvokeAsync_NoException_CallsNextAndReturnsNormally()
    {
        var nextCalled = false;
        var middleware = CreateMiddleware(_ => { nextCalled = true; return Task.CompletedTask; });
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_BadHttpRequestException_Returns400()
    {
        var middleware = CreateMiddleware(_ => throw new BadHttpRequestException("Name is required."));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_BadHttpRequestException_ReturnsJsonEnvelope()
    {
        var middleware = CreateMiddleware(_ => throw new BadHttpRequestException("Name is required."));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBody(context);
        Assert.NotNull(body);
        Assert.False(body.Success);
        Assert.Equal("Name is required.", body.Message);
        Assert.Equal((int)HttpStatusCode.BadRequest, body.ResponseCode);
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_Returns500()
    {
        var middleware = CreateMiddleware(_ => throw new InvalidOperationException("Something broke"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_ReturnsGenericMessage()
    {
        var middleware = CreateMiddleware(_ => throw new InvalidOperationException("db connection failed"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBody(context);
        Assert.NotNull(body);
        Assert.False(body.Success);
        Assert.Equal("An unexpected error occurred.", body.Message);
        Assert.Equal((int)HttpStatusCode.InternalServerError, body.ResponseCode);
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_DoesNotLeakExceptionDetails()
    {
        var middleware = CreateMiddleware(_ => throw new InvalidOperationException("secret connection string"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBody(context);
        Assert.NotNull(body);
        Assert.DoesNotContain("secret", body.Message);
    }

    [Fact]
    public async Task InvokeAsync_BadRequest_LogsToLogService()
    {
        var logService = new MockLogService();
        var middleware = CreateMiddleware(_ => throw new BadHttpRequestException("Invalid input"));
        var context = CreateHttpContext(logService);

        await middleware.InvokeAsync(context);

        Assert.Single(logService.ExceptionLogs);
        Assert.Equal("ExceptionHandlingMiddleware", logService.ExceptionLogs[0].Source);
        Assert.Equal("Invalid input", logService.ExceptionLogs[0].Exception.Message);
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_LogsToLogService()
    {
        var logService = new MockLogService();
        var middleware = CreateMiddleware(_ => throw new InvalidOperationException("Kaboom"));
        var context = CreateHttpContext(logService);

        await middleware.InvokeAsync(context);

        Assert.Single(logService.ExceptionLogs);
        Assert.Equal("ExceptionHandlingMiddleware", logService.ExceptionLogs[0].Source);
    }

    [Fact]
    public async Task InvokeAsync_ResponseContentType_IsJson()
    {
        var middleware = CreateMiddleware(_ => throw new Exception("test"));
        var context = CreateHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_NoLogService_DoesNotThrow()
    {
        var middleware = CreateMiddleware(_ => throw new Exception("test"));
        var context = CreateHttpContext(logService: null);

        await middleware.InvokeAsync(context);

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
    }
}