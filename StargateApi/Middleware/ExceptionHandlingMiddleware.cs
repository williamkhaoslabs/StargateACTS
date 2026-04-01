using System.Net;
using System.Text.Json;
using StargateApi.Business.Services;
using StargateAPI.Controllers;

namespace API.Middleware;

public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
 
        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
 
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);
                
                var logService = context.RequestServices.GetService<ILogService>();
                if (logService != null)
                {
                    await logService.LogException(ex, "ExceptionHandlingMiddleware");
                }
 
                await WriteResponse(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                
                var logService = context.RequestServices.GetService<ILogService>();
                if (logService != null)
                {
                    await logService.LogException(ex, "ExceptionHandlingMiddleware");
                }
 
                await WriteResponse(context, HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.");
            }
        }
 
        private static async Task WriteResponse(
            HttpContext context, HttpStatusCode code, string message)
        {
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";
 
            var response = new BaseResponse
            {
                Success = false,
                Message = message,
                ResponseCode = (int)code
            };
 
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }